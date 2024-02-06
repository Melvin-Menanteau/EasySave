using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EasySave
{
    public class GetMethodFromCommand
    {
        public SaveConfiguration Configuration;

        public List<Save> Saves = [];

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
                List<int> IdsSauvegarde = [];

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
                else
                {
                    foreach(var id in IdsSauvegarde)
                    {
                        Console.WriteLine(id);
                    }
                }
            }
            else if (command.StartsWith("cls"))
            {
                Console.Clear();
            }
            else if (command.StartsWith("lang"))
            {
                string lang = GetLangFromCommand(command);

                if (lang.ToLower() == "en" || lang.ToLower() == "fr")
                {
                    SharedLocalizer.SetCulture(lang);
                    Console.WriteLine(SharedLocalizer.GetLocalizedString("Langage"));
                }
                else
                {
                    Console.WriteLine("Erreur - Commande LANG non reconnue");
                }
            }
            else if (command.StartsWith("help"))
            {
                
            }
            else if (command.StartsWith("cs"))
            {
                Saves = Configuration.GetConfiguration();

                try
                {
                    string nom = GetNomFromCommand(command);

                    if (nom == null) { return; }

                    string InputFolder = GetInputFolderFromCommand(command);

                    if (InputFolder == null) { return; }

                    string OutputFolder = GetOutputFolderFromCommand(command);

                    if (OutputFolder == null) { return; }

                    SaveType TypeSave = GetTypeOfSaveFromCommand(command);

                    Configuration.AddConfiguration(nom, InputFolder, OutputFolder, TypeSave);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public string GetLangFromCommand(string command)
        {
            string lang = "";

            Regex regex = new Regex(@"lang\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                lang = match.Groups[1].Value;
            }
            else if (command.Contains("lang"))
            {
                Console.WriteLine("Erreur - Regex non conforme");
                lang = null;
            }

            return lang;
        }

        public string GetInputFolderFromCommand(string command)
        {
            string path ="";

            Regex regex = new Regex(@"-if\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                path = match.Groups[1].Value;
            }
            else if (command.Contains("-if"))
            {
                Console.WriteLine("Erreur - Regex non conforme");
                path = null;
            }

            return path;
        }

        public string GetOutputFolderFromCommand(string command)
        {
            string path = "";

            Regex regex = new Regex(@"-of\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                path = match.Groups[1].Value;
            }
            else if (command.Contains("-of"))
            {
                Console.WriteLine("Erreur - Regex non conforme");
                path = null;
            }

            return path;
        }
        public SaveType GetTypeOfSaveFromCommand(string command)
        {
            string typeString = "";
            SaveType type = new();

            Regex regex = new Regex(@"-t\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                typeString = match.Groups[1].Value;
                if (typeString.ToUpper() == "COMPLETE")
                {
                    type = SaveType.COMPLETE;
                }
                else if (typeString.ToUpper() == "DIFFERENTIAL")
                {
                    type = SaveType.DIFFERENTIAL;
                }
            }
            else if (command.Contains("-t"))
            {
                Console.WriteLine("Erreur - Regex non conforme");
                typeString = null;
            }

            return type;
        }

        public List<int> GetAllIdsFromCommand()
        {
            List<int> Ids = [];

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
            List<int> Ids = [];
            string IdsString = "";

            Saves = Configuration.GetConfiguration();

            Regex regex = new Regex(@"-id\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                IdsString = match.Groups[1].Value;


                if(IdsString.Contains('-') && IdsString.Contains(';'))
                {
                    Console.WriteLine("Erreur - La commande LS ne peut pas contenir - et ;");
                }
                else if (IdsString.Contains('-'))
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
                else if (IdsString.Contains(';'))
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
