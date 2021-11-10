using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace AutoBackup
{
    public class Handler
    {
        string source;
        string target;

        DbProvider provider;

        public Handler(String source, String target)
        {
            this.source = source;
            this.target = target;

            this.provider = new DbProvider();
        }

        public Status Add(String name)
        {
            FileModel sourceFile = provider.Get(name);
            FileInfo fileInfo = new FileInfo(Path.Combine(this.source, name));
            if (sourceFile == null)
            {
                return Add(fileInfo);
            }
            else
            {
                return Update(fileInfo);
            }
        }

        public void Delete(String name)
        {
            FileModel file = provider.Get(name);

            if (file.Status == Status.CopyFailed)
            {
                file.Status = Status.SourceMissing;
            }
            else
            {
                file.Status = Status.Deleted;
            }

            provider.Update(file);
        }


        public void SyncAll()
        {
            var stored = this.provider.Getall();

            var files = Directory.GetFiles(this.source);

            foreach (var item in files)
            {
                string name = Path.GetFileName(item);
                FileInfo file = new FileInfo(item);
                FileModel source;

                // the file is old
                if (stored.TryGetValue(name, out source))
                {
                    // file is not changed
                    if (file.LastWriteTime == source.LastModifiedTime)
                    {
                        stored.Remove(name);
                        continue;
                    }
                    // file is updated               
                    Update(file);

                    stored.Remove(name);
                    continue;
                }
                Add(file);
            }

            // File is deleted
            foreach (var item in stored)
            {
                FileModel file = item.Value;
                if (file.Status == Status.Copied)
                {
                    file.Status = Status.Deleted;
                }
                else if (file.Status == Status.CopyFailed)
                {
                    file.Status = Status.SourceMissing;
                }
                else
                {
                    continue;
                }
                provider.Update(file);
            }
        }

        private Status CopyFile(FileInfo fileInfo)
        {
            try
            {
                File.Copy(fileInfo.FullName, Path.Combine(this.target, fileInfo.Name), true);
                return Status.Copied;
            }
            catch (IOException ex)
            {
                return Status.CopyFailed;
            }
        }

        private Status DecryptNCM(FileInfo file)
        {
            try
            {
                NeteaseCrypto netease = new NeteaseCrypto(file);
                netease.Dump(this.target);

                return Status.Copied;
            }
            catch (IOException ex)
            {
                return Status.ConvertFailed;
            }

        }

        private Status Add(FileInfo file)
        {
            FileModel model = OperateFile(file);

            this.provider.Add(model);

            return model.Status;
        }

        private Status Update(FileInfo file)
        {
            FileModel model = OperateFile(file);

            this.provider.Update(model);

            return model.Status;
        }

        private FileModel OperateFile(FileInfo file)
        {
            Status status;
            if (Path.GetExtension(file.FullName) == ".ncm")
            {
                status = DecryptNCM(file);
            }
            else
            {
                status = CopyFile(file);
            }

            FileModel model = new FileModel();
            model.Name = file.Name;
            model.LastModifiedTime = file.LastWriteTime;
            model.Status = status;
            return model;
        }
    }
}
