
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
namespace HomeWork_11.Models
{
    class OrganizationBase 
    {
        private Department dep;
        private string currentPath;
        private Random rand = new Random();
        public bool IsSaved { get; set; }
        public Department GetOrganization()
        {
            return dep;
        }

        public OrganizationBase()
        {
            Load("base.json");
        }

        public OrganizationBase(string path)
        {
            //dep = new Department("");
            Load(path);
            if (dep == null) dep = new Department("");
        }


        public void Load(string path)
        {
            currentPath = path;
            string json;
            using (Stream st = File.Open(path, FileMode.OpenOrCreate))
            {
                StreamReader sr = new StreamReader(st);
                json = sr.ReadToEnd();
            }

            dep = JsonConvert.DeserializeObject<Department>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            IsSaved = true;
        }

        public void Save()
        {
            var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(dep, Formatting.Indented, jset);
            if (currentPath == string.Empty || currentPath =="base.json")
            {
                currentPath = "base.json";
                
                using (StreamWriter sw = new StreamWriter("base.json"))
                {
                    sw.WriteLine(json);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(currentPath))
                {
                    sw.WriteLine(json);
                }
                File.Copy(currentPath, "base.json", true);
            }
        
            IsSaved = true;
        }

        public void Save(string path)
        {
            var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(dep, Formatting.Indented, jset);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(json);
            }

            File.Copy(path, "base.json", true);
            IsSaved = true;
        }

        public void RandomBaseGenerator()
        {
            var randomOrg = new Department("Главный департамент случайных департаментов");
            randomOrg.AddWorker(new HighManager(lastnames[rand.Next(0, 6)],
               names[rand.Next(0, 6)], $"Директор {randomOrg.DepartmentName}", (byte)rand.Next(40, 70), DateTime.Now, randomOrg
               ));
            for (int i = 1; i < 6;i++)
            {
                randomOrg.AddSubDepartment(randomDep());
            }
            this.dep = randomOrg;
        }

        private Department randomDep ()
        {
            var newDep = new Department($"Отдел №{rand.Next(Math.Abs((int)DateTime.Now.Ticks))}");
            randomWorkers(newDep);
            for (int i = 2; i < rand.Next(2, 4); i++)
            {
               newDep.AddSubDepartment(randomDep());
            }
            return newDep;
        }

        private void randomWorkers(Department inputDep)
        {
            inputDep.AddWorker(new HighManager(lastnames[rand.Next(0,6)],
                names[rand.Next(0,6)],$"Начальник {inputDep.DepartmentName}",(byte)rand.Next(40,70),DateTime.Now,inputDep
                ));
            for(int i= 0; i<rand.Next(1,5);i++)
            {
                inputDep.AddWorker(new Manager(lastnames[rand.Next(0, 6)],
                names[rand.Next(0, 6)], $"Специалист {inputDep.DepartmentName}", (byte)rand.Next(18, 70), DateTime.Now,
                (ushort)rand.Next(150, 200), (ushort)rand.Next(5, 20)
                ));
                if (rand.Next(0, 2) == 0)
                {
                    inputDep.AddWorker(new Intern(lastnames[rand.Next(0, 6)],
                names[rand.Next(0, 6)], (byte)rand.Next(18, 70), DateTime.Now, DateTime.Now
                ));
                }
            }
        }

        Dictionary<int, string> names = new Dictionary<int, string>()
        {
            {0,"Михаил"},
            {1,"Кирилл"},
            {2,"Сергей"},
            {3,"Василий"},
            {4,"Геннадий"},
            {5,"Станистав"}
        };
        Dictionary<int, string> lastnames = new Dictionary<int, string>()
        {
            {0,"Петров"},
            {1,"Иванов"},
            {2,"Сидоров"},
            {3,"Синьков"},
            {4,"Ленин"},
            {5,"Сталин"}
        };
    }
}
