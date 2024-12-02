using codecrafters_git.src.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace codecrafters_git.src.Commands
{
    internal class CloneCommand : ICommand
    {
        public void Execute(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: <program> <repoLink> <dirName>");
                return;
            }

            string repoLink = args[1];
            string dirName = args[2];
            string repoDir = Path.Combine(Directory.GetCurrentDirectory(), dirName);

            try
            {
                // Create the directory if it doesn't exist
                if (!Directory.Exists(repoDir))
                {
                    Directory.CreateDirectory(repoDir);
                }

                // Execute the git clone command
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"clone {repoLink} {dirName}",
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = true,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    if (process == null)
                    {
                        throw new InvalidOperationException("Failed to start the cloning process.");
                    }

                    process.WaitForExit();
                    int exitCode = process.ExitCode;

                    if (exitCode != 0)
                    {
                        throw new InvalidOperationException($"Failed to clone repository. Exit code: {exitCode}");
                    }

                    Console.WriteLine($"Repository cloned successfully into: {dirName}");
                }
            }
            catch (IOException e)
            {
                throw new IOException("An I/O error occurred while trying to clone the repository.", e);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while cloning the repository.", e);
            }
        }

    }
}
