using Chreytli.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Chreytli.Api.BusinessControllers
{
    public class SubmissionsBusinessController
    {
        public void GetThumbnail(ref Submission submission, string mimeType)
        {
            if (submission.Type == SubmissionTypes.Image || submission.Type == SubmissionTypes.Video)
            {
                //var request = WebRequest.Create(url) as HttpWebRequest;
                //if (request != null)
                //{
                //    var response = request.GetResponse() as HttpWebResponse;

                //    if (response != null)
                //        submission.MimeType = response.ContentType;
                //}

                var mime = mimeType.Split('/');

                //if (mime[0] == "audio" || mime[0] == "video")
                //{
                //    submission.Type = SubmissionTypes.Video;
                //}
                //else
                //{
                //    SubmissionTypes type;
                //    if (Enum.TryParse(mime[0], true, out type))
                //        submission.Type = type;
                //}

                //var regex = new Regex(@"data:image\/(.*);base64,(.*)");
                //var match = regex.Match(submission.Img);

                var fileName = "images/posts/" + submission.AuthorId + "-" + submission.Date.ToBinary() + "." + mime[1];
                var imgName = "images/posts/" + submission.AuthorId + "-" + submission.Date.ToBinary() + ".jpg";

                //if (match.Success)
                //{
                //    var blob = Convert.FromBase64String(match.Groups[2].Value);
                //    File.WriteAllBytes(fileNameOnServer, blob);
                //    submission.Img = fileName;
                //}
                //else
                //{

                // Create folders if they don't exist already
                if (!Directory.Exists(GetServerPath("images")))
                {
                    Directory.CreateDirectory(GetServerPath("images"));
                }
                if (!Directory.Exists(GetServerPath("images/posts")))
                {
                    Directory.CreateDirectory(GetServerPath("images/posts"));
                }

                if (submission.IsHosted)
                {
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadFile(submission.Url, GetServerPath(fileName));
                        //webClient.DownloadFile(submission.Img, GetServerPath(imgName));
                        submission.Url = fileName;
                    }
                }

                submission.Img = submission.Url;

                try
                {
                    if (submission.Type == SubmissionTypes.Image && mime[1] == "gif")
                    {
                        submission.Img = imgName;

                        var path = submission.IsHosted ? GetServerPath(submission.Url) : submission.Url;
                        using (var img = System.Drawing.Image.FromFile(path))
                        {
                            var imgPath = GetServerPath(submission.Img);
                            img.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                    else if (submission.Type == SubmissionTypes.Video)
                    {
                        submission.Img = imgName;

                        var path = submission.IsHosted ? GetServerPath(submission.Url) : submission.Url;
                        var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                        ffMpeg.GetVideoThumbnail(path, GetServerPath(submission.Img), 0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException());
                }
                //}
            }
        }

        private string GetServerPath(string path)
        {
            return HostingEnvironment.MapPath("~/" + path);
        }
    }
}