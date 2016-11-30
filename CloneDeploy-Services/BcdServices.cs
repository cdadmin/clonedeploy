using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CloneDeploy_App.Helpers;
using CloneDeploy_Entities;

namespace CloneDeploy_App.BLL
{
    public class BcdServices
    {
        /*http://www.geoffchappell.com/notes/windows/boot/bcd/objects.htm
        None of these are used but may be handy in the future
        private const string BOOT_MGR = "9dea862c-5cdd-4e70-acc1-f32b344d4795";
        private const string FWBOOT_MGR = "a5a30fa2-3d06-4e9f-b5f4-a01df9d1fcba";
        private const string MEM_DIAG = "2721d73-1db4-4c62-bf78-c548a880142d";
        private const string RESUME = "147aa509-0358-4473-b83b-d950dda00615";
        private const string NTLDR = "466f5a88-0af2-4f76-9038-095b170dc21c";
        private const string CURRENT = "fa926493-6f1c-4193-a414-58f0b2456d1e";
        */

        public string UpdateEntry(string bcd, long newOffsetBytes)
        {
            var charOffsetHex = newOffsetBytes.ToString("X16").Reverse().ToList();
            var output = new StringBuilder();

            for (var i = 0; i < charOffsetHex.Count; i++)
            {
                if (i%2 != 0) continue;
                if ((i + 1) < charOffsetHex.Count)
                {
                    output.Append(charOffsetHex[i + 1]);
                }
                output.Append(charOffsetHex[i]);

                if (i + 2 != charOffsetHex.Count)
                    output.Append(",");
            }
            var newOffsetHex = output.ToString();

            var guids = new List<string>();
            var regfile = new RegFileObject(bcd);
            foreach (var entry in regfile.RegValues)
            {
                foreach (var value in entry.Value.Values)
                {
                    if (!value.Value.ToLower().Contains("winload.exe")) continue;
                    var matches = Regex.Matches(entry.Key, @"\{(.*?)\}");
                    guids.AddRange(from Match m in matches select m.Groups[1].Value);
                }
            }

            guids = guids.Distinct().ToList();
            foreach (var guid in guids)
            {
                var regBinary =
                    regfile.RegValues[@".\Objects\{" + guid + @"}\Elements\11000001"]["Element"]
                        .Value;
                var regBinarySplit = regBinary.Split(',');
                var originalOffsetHex = regBinarySplit[32] + "," + regBinarySplit[33] + "," + regBinarySplit[34] + "," + regBinarySplit[35] + "," +
                               regBinarySplit[36] + "," + regBinarySplit[37] + "," + regBinarySplit[38] + "," + regBinarySplit[39];

                var regex = new Regex(originalOffsetHex, RegexOptions.IgnoreCase);
                bcd = regex.Replace(bcd, newOffsetHex);

            }


            return Utility.Encode(bcd);
        }
    }
}