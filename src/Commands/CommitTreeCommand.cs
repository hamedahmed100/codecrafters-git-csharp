using codecrafters_git.src.Commands.Interfaces;
using codecrafters_git.src.Services;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_git.src.Commands
{
    public class CommitTreeCommand : ICommand
    {
        private readonly GitObjectStore _gitObjectStore;
        private readonly HashCalculator _hashCalculator;

        public CommitTreeCommand()
        {
            _gitObjectStore = new GitObjectStore();
            _hashCalculator = new HashCalculator();
        }
        public void Execute(string[] args)
        {
            if (args.Length < 6 || args[2] != "-p" || args[4] != "-m")
                throw new ArgumentException("Invalid arguments for commit-tree");

            string treeSha = args[1];
            string parentSha = args[3];
            string message = args[5];

            string authorName = "Your Name";
            string authorEmail = "your_email@example.com";
            string committerName = "Your Name";
            string committerEmail = "your_email@example.com";

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string timezone = "+0000";

            string commitContent = BuildCommitContent(
                treeSha,
                parentSha,
                authorName,
                authorEmail,
                committerName,
                committerEmail,
                timestamp,
                timezone,
                message
            );

            string commitSha = SaveCommitObject(commitContent);
            Console.WriteLine(commitSha);
        }

        private string BuildCommitContent(
            string treeSha,
            string parentSha,
            string authorName,
            string authorEmail,
            string committerName,
            string committerEmail,
            long timestamp,
            string timezone,
            string message)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"tree {treeSha}");
            builder.AppendLine($"parent {parentSha}");
            builder.AppendLine($"author {authorName} <{authorEmail}> {timestamp} {timezone}");
            builder.AppendLine($"committer {committerName} <{committerEmail}> {timestamp} {timezone}");
            builder.AppendLine();
            builder.AppendLine(message);

            return builder.ToString();
        }

        private string SaveCommitObject(string commitContent)
        {
            string header = $"commit {commitContent.Length}\0";
            byte[] commitBytes = Encoding.UTF8.GetBytes(header + commitContent);

            string sha = _hashCalculator.ComputeSHA1(commitBytes);
            _gitObjectStore.SaveObject(commitBytes, sha);

            return sha;
        }
    }

}

