using codecrafters_git.src.Commands.Interfaces;
using codecrafters_git.src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_git.src.Commands
{
    public class LsTreeNameOnlyCommand : ICommand
    {
        private readonly GitObjectStore _gitObjectStore;

        public LsTreeNameOnlyCommand()
        {
            _gitObjectStore = new GitObjectStore();
        }
        public void Execute(string[] args)
        {
            if (args.Length < 3 || args[1] != "--name-only")
                throw new ArgumentException(
                    "Invalid arguments for ls-tree. Usage: git ls-tree --name-only <tree_sha>");
            string treeSha = args[2];
            try
            {
                // Fetch the tree object
                byte[] treeData = _gitObjectStore.ReadObject(treeSha);
                var stringFileContent = Encoding.UTF8.GetString(treeData);
                var nullSplitFileContent = stringFileContent.Split("\0");
                var filenames = nullSplitFileContent.Skip(1)
                                    .Select(s => s.Split(" ").Last())
                                    .SkipLast(1);
                foreach (var filename in filenames)
                {
                    Console.WriteLine(filename);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }



        }
    }
}
