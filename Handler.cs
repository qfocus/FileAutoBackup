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
            SourceFile sourceFile = provider.Get(name);
            if (sourceFile == null)
            {
                sourceFile = new SourceFile
                {
                    Name = name,
                    LastModifiedTime = File.GetLastWriteTime(Path.Combine(this.source, name))
                };

                Add(sourceFile);
            }
            else
            {
                Update(sourceFile);
            }
            return sourceFile.Status;
        }

        public void Delete(String name)
        {
            SourceFile file = provider.Get(name);

            if (file.Status == Status.Failed)
            {
                file.Status = Status.Missing;
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
                SourceFile source;

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
                    source.LastModifiedTime = file.LastWriteTime;

                    Update(source);

                    stored.Remove(name);
                    continue;
                }
                // the file is new
                source = new SourceFile
                {
                    LastModifiedTime = file.LastWriteTime,
                    Name = name
                };

                Add(source);
            }

            // File is deleted
            foreach (var item in stored)
            {
                SourceFile file = item.Value;
                if (file.Status == Status.Copied)
                {
                    file.Status = Status.Deleted;
                }
                else if (file.Status == Status.Failed)
                {
                    file.Status = Status.Missing;
                }
                else
                {
                    continue;
                }
                provider.Update(file);
            }
        }

        private void CopyFile(SourceFile source)
        {
            try
            {
                File.Copy(Path.Combine(this.source, source.Name), Path.Combine(this.target, source.Name), true);
                source.Status = Status.Copied;
            }
            catch (IOException ex)
            {
                source.Status = Status.Failed;
            }
        }

        private void Add(SourceFile source)
        {
            if (!File.Exists(Path.Combine(this.source, source.Name)))
            {
                return;
            }
            CopyFile(source);

            this.provider.Add(source);
        }

        private void Update(SourceFile source)
        {
            CopyFile(source);

            this.provider.Update(source);
        }


    }
}
