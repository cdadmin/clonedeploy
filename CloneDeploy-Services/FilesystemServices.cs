using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities.DTOs;

#if __MonoCS__  
using Mono.Unix; // requires reference to  Mono.Posix.dll
#endif

namespace CloneDeploy_Services
{
    public class FilesystemServices
    {
        public DpFreeSpaceDTO GetDpFreeSpace()
        {
            var primaryDp = new DistributionPointServices().GetPrimaryDistributionPoint();

            var dpFreeSpace = new DpFreeSpaceDTO();
            dpFreeSpace.dPPath = primaryDp.PhysicalPath;

            if (System.IO.Directory.Exists(primaryDp.PhysicalPath))
            {
                ulong freespace = 0;
                ulong total = 0;
                bool success = DriveFreeBytes(primaryDp.PhysicalPath, out freespace, out total);

                if (!success) return null;

                int freePercent = 0;
                int usedPercent = 0;

                if (total > 0 && freespace > 0)
                {
                    freePercent = (int)(0.5f + ((100f * Convert.ToInt64(freespace)) / Convert.ToInt64(total)));
                    usedPercent =
                        (int)(0.5f + ((100f * Convert.ToInt64(total - freespace)) / Convert.ToInt64(total)));
                }
                dpFreeSpace.freespace = freespace;
                dpFreeSpace.total = total;
                dpFreeSpace.freePercent = freePercent;
                dpFreeSpace.usedPercent = usedPercent;
            }

            return dpFreeSpace;

        }
        // if running on Mono
#if __MonoCS__     
        public static bool DriveFreeBytes(string folderName, out ulong freespace, out ulong total)
        {
            freespace = 0;
            total = 0;
            
            UnixDriveInfo[] drives = UnixDriveInfo.GetDrives();
            int idx = -1, count = -1;
            for (int i = 0; i < drives.Length; ++i)
            {
              if (folderName.StartsWith (drives[i].Name) && drives[i].Name.Length > count)
              {
                count = drives[i].Name.Length;
                idx = i;
              }
            }
            
            // Drive for path is: drives[idx].Name
            freespace = (ulong)drives[idx].AvailableFreeSpace;
            total = (ulong)drives[idx].TotalSize; 
            return true;
        }   
#else

        // using GetDiskFreeSpaceEx because this handles mountpoints, quota and UNC 
        // there are reports that old CIFS doesn't support unc-share to a mountpoint, needs Windows 2008/SMB2        
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

        public static bool DriveFreeBytes(string folderName, out ulong freespace, out ulong total)
        {
            freespace = 0;
            total = 0;
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (!folderName.EndsWith("\\"))
            {
                folderName += '\\';
            }

            ulong free = 0, tot = 0, dummy2 = 0;

            if (GetDiskFreeSpaceEx(folderName, out free, out tot, out dummy2))
            {
                freespace = free;
                total = tot;
                return true;
            }
            else
            {
                return false;
            }
        }
#endif
    }
}
