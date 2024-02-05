using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EasySave
{
    public class GetMethodFromCommand
    {
        public SaveConfiguration Configuration;

        public List<Save> Saves = new List<Save>();

        public GetMethodFromCommand()
        {
            Configuration = SaveConfiguration.GetInstance();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void GetMethod(string command)
        {
            if (command.StartsWith("ls"))
            {
                List<int> IdsSauvegarde = new List<int>();

                if (command.Contains("-n"))
                {
                    string NomSauvegarde = GetNomFromCommand(command);

                    if (NomSauvegarde == null)
                    {
                        Console.WriteLine("Erreur - Commande LS non reconnue");
                    }

                    int IdSauvegarde = GetIdFromNom(NomSauvegarde);

                    if (IdSauvegarde > 0)
                    {
                        IdsSauvegarde.Add(IdSauvegarde);
                    }
                    else
                    {
                        Console.WriteLine("Erreur - Le nom de la sauvegarde n'existe pas ");
                    }
                }
                else if (command.Contains("-id"))
                {
                    IdsSauvegarde = GetIdsFromCommand(command);
                }
                else if (command.Contains("-all"))
                {
                    IdsSauvegarde = GetAllIdsFromCommand();
                }

                if (IdsSauvegarde.Count == 0)
                {
                    Console.WriteLine("Erreur - Commande LS non reconnue");
                }
            }
        }

        public List<int> GetAllIdsFromCommand()
        {
            List<int> Ids = new List<int>();

            Saves = Configuration.GetConfiguration();

            foreach (var save in Saves)
            {
                Ids.Add(save.Id);
            }

            return Ids;
        }

        public string GetNomFromCommand(string command)
        {
            string nom = "";

            Regex regex = new Regex(@"-n\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                nom = match.Groups[1].Value;
            }
            else if (command.Contains("-n"))
            {
                Console.WriteLine("Erreur - Regex non conforme");
                nom = null;
            }

            return nom;
        }

        public int GetIdFromNom(string nom)
        {
            int Id = -1;

            Saves = Configuration.GetConfiguration();

            foreach (Save save in Saves)
            {
                if (save.Name == nom)
                {
                    Id = save.Id;
                }
            }

            return Id;
        }

        public List<int> GetIdsFromCommand(string command)
        {
            List<int> Ids = new List<int>();
            string IdsString = "";

            Saves = Configuration.GetConfiguration();

            Regex regex = new Regex(@"-id\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                IdsString = match.Groups[1].Value;


                if(IdsString.Contains("-") && IdsString.Contains(";"))
                {
                    Console.WriteLine("Erreur - La commande LS ne peut pas contenir - et ;");
                }
                else if (IdsString.Contains("-"))
                {
                    string[] Id = IdsString.Split('-');

                    int StartId = int.Parse(Id[0]);
                    int EndId = int.Parse(Id[1]);

                    if (StartId > EndId)
                    {
                        (EndId, StartId) = (StartId, EndId);
                    }

                    foreach (Save save in Saves)
                    {
                        if (StartId <= save.Id && save.Id <= EndId)
                        {
                            Ids.Add(save.Id);
                        }
                    }
                }
                else if (IdsString.Contains(";"))
                {
                    string[] IdString = IdsString.Split(';');
                    int[] IdInt = new int[IdString.Length];
                    try
                    {
                        IdInt = IdString.Select(int.Parse).ToArray();
                        foreach(int id in IdInt)
                        {
                            foreach (Save save in Saves)
                            {
                                if (id == save.Id)
                                {
                                    Ids.Add(id); 
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else if (int.TryParse(IdsString, out int id))
                {
                    Ids.Add(id);
                }
            }
            else
            {
                Console.WriteLine("Erreur - Regex non conforme");
            }

            return Ids;
        }
    }
}
