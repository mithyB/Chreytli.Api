using Chreytli.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Data.Entity;

namespace Chreytli.Api.BusinessControllers
{
    public class SubmissionsBusinessController
    {
        public void GetThumbnail(ref Submission submission, string mimeType)
        {
            if (submission.Type == SubmissionTypes.Image || submission.Type == SubmissionTypes.Video)
            {
                var mime = mimeType.Split('/');

                var fileName = "images/posts/" + submission.AuthorId + "-" + submission.Date.ToBinary() + "." + mime[1];
                var imgName = "images/posts/" + submission.AuthorId + "-" + submission.Date.ToBinary() + ".jpg";

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
            }
        }

        internal IQueryable<Submission> GetFilteredSubmissions(DbSet<Submission> submissions, string[] filter)
        {
            if (filter == null || filter.Length == 0)
            {
                filter = new [] { "sfw" };
            }

            return submissions.Where(x => filter.Any(f => f.ToLower() == x.Tag.ToString().ToLower()));
        }

        public void RemoveImages(Submission submission)
        {
            if (!submission.Img.StartsWith("http") && File.Exists(GetServerPath(submission.Img)))
            {
                File.Delete(GetServerPath(submission.Img));
            }
            if (!submission.Url.StartsWith("http") && File.Exists(GetServerPath(submission.Url)))
            {
                File.Delete(GetServerPath(submission.Url));
            }
        }

        private string GetServerPath(string path)
        {
            return HostingEnvironment.MapPath("~/" + path);
        }
    }
}