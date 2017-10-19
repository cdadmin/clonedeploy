using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using CloneDeploy_Entities.DTOs;
using Newtonsoft.Json.Linq;

namespace CloneDeploy_Services
{
    public class FileUploadServices
    {
        private readonly FileUploadDTO _upload;


        public FileUploadServices(FileUploadDTO upload)
        {
            _upload = upload;

        }

        public string Upload()
        {
            if (_upload.TotalParts > 0)
            {
                if (_upload.UploadMethod == "alternative")
                    return SaveBlobAlternate();
                else

                    return SaveBlob();
            }
            else
            {
                var createDirResult = CreateDirectory();
                if (createDirResult != null) return createDirResult;
                return SaveAs();
            }
        }


        private string SaveAs(string destination = null)
        {
            var filePath = destination ?? Path.Combine(_upload.DestinationDirectory, _upload.Filename);

            try
            {
                using (var file = new FileStream(filePath, FileMode.CreateNew))
                    _upload.InputStream.CopyTo(file);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }



        private string SaveBlob()
        {
            var filePath = Path.Combine(_upload.DestinationDirectory, _upload.OriginalFilename);
            if (_upload.PartIndex == 0)
            {
                var createDirResult = CreateDirectory();
                if (createDirResult != null) return createDirResult;

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Stream stream = null;
            try
            {
                stream = new FileStream(filePath, (_upload.PartIndex == 0) ? FileMode.CreateNew : FileMode.Append);
                _upload.InputStream.CopyTo(stream, 16384);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();

            }
            return null;
        }

        private string SaveBlobAlternate()
        {
            var path = Path.Combine(_upload.DestinationDirectory, _upload.PartUuid + "." + _upload.PartIndex);
            SaveAs(path);


            if (_upload.PartIndex == (_upload.TotalParts - 1))
            {
                ulong bytesWritten = 0;
                using (
                    var output = System.IO.File.OpenWrite(Path.Combine(_upload.DestinationDirectory, _upload.OriginalFilename))
                    )
                {
                    for (var i = 0; i < _upload.TotalParts; i++)
                    {

                        using (
                            var input = System.IO.File.OpenRead(Path.Combine(_upload.DestinationDirectory, _upload.PartUuid + "." + i))
                            )
                        {
                            var buff = new byte[1];
                            while (input.Read(buff, 0, 1) > 0)
                            {
                                output.WriteByte(buff[0]);
                                bytesWritten++;
                            }
                            input.Close();
                        }
                        output.Flush();
                    }
                    output.Close();

                    if (bytesWritten != _upload.FileSize)
                    {
                        return "Filesize Mismatch";
                    }

                    for (var i = 0; i < _upload.TotalParts; i++)
                    {
                        System.IO.File.Delete(Path.Combine(_upload.DestinationDirectory, _upload.PartUuid + "." + i));
                    }

                }

            }
            return null;

        }

        private string CreateDirectory()
        {
            var directory = new DirectoryInfo(_upload.DestinationDirectory);
            try
            {
                if (!directory.Exists)
                    directory.Create();
                    return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }

    public class FineUploaderResult
    {
        private readonly bool _success;
        private readonly string _error;
        private readonly bool? _preventRetry;
        private readonly JObject _otherData;

        public FineUploaderResult(bool success, object otherData = null, string error = null, bool? preventRetry = null)
        {
            _success = success;
            _error = error;
            _preventRetry = preventRetry;

            if (otherData != null)
                _otherData = JObject.FromObject(otherData);
        }


        public string BuildResponse()
        {
            var response = _otherData ?? new JObject();
            response["success"] = _success;

            if (!string.IsNullOrWhiteSpace(_error))
                response["error"] = _error;

            if (_preventRetry.HasValue)
                response["preventRetry"] = _preventRetry.Value;

            return response.ToString();
        }
    }
}
