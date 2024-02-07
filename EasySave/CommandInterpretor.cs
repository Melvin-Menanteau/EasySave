using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EasySave
{
    public partial class CommandInterpretor
    {
        private static readonly SaveConfiguration _saveConfiguration = SaveConfiguration.GetInstance();
        private static readonly EasySave _easySave = new EasySave();

        // Regex lecture commande = https://regex101.com/r/zO8w16/1
        [GeneratedRegex("^(?'cmd'\\w+).?(?'args'.*)?$", RegexOptions.IgnoreCase)]
        private static partial Regex _regexCommand();

        // Regex ids sauvegardes = https://regex101.com/r/ZhWSGv/1
        [GeneratedRegex("-id\\s+\"?((?'ids'[\\d-]+);?)+\"?", RegexOptions.IgnoreCase)]
        private static partial Regex _regexIds();

        // Regex param commande = https://regex101.com/r/IjrFju/1
        [GeneratedRegex("""(?'param'-(?'paramName'\w+)(\s"?(?'paramValue'[^"]+))?"?\s?)+""", RegexOptions.IgnoreCase)]
        private static partial Regex _regexParam();

        /// <summary>
        /// Lire et executer une commande
        /// </summary>
        /// <param name="command">Commande rentree par l'utilisateur</param>
        /// <exception cref="ArgumentException">La commande n'est pas reconnue</exception>
        public static void ReadCommand(string command)
        {
            if (_regexCommand().IsMatch(command))
            {
                string cmd = _regexCommand().Match(command).Groups["cmd"].Value.ToLower();
                string args = _regexCommand().Match(command).Groups["args"].Value;

                Dictionary<string, string> parametres = ExtraireParametre(args);

                if (parametres.ContainsKey("lang"))
                {
                    ChangerLangue(parametres["lang"]);
                }

                switch (cmd)
                {
                    case "run":
                    case "r":
                        LancerSauvegarde(args);
                        break;
                    case "list":
                    case "ls":
                        ListerSauvegardes();
                        break;
                    case "add":
                        AjouterConfiguration(args);
                        break;
                    case "update":
                        ModifierConfiguration(args);
                        break;
                    case "remove":
                        SupprimerConfiguration(args);
                        break;
                    case "lang":
                        break;
                    case "help":
                    case "h":
                        AfficherAide();
                        break;
                    case "clear":
                    case "cls":
                        Console.Clear();
                        break;
                    default:
                        throw new ArgumentException(SharedLocalizer.GetLocalizedString("UnrecognizedCommand"));
                }
            }
            else
            {
                throw new ArgumentException(SharedLocalizer.GetLocalizedString("CommandParseException"));
            }
        }

        /// <summary>
        /// Extraire les parametres d'une commande sous forme de dictionnaire (nom du parametre, valeur du parametre)
        /// </summary>
        /// <param name="args">Arguments de la commande</param>
        private static Dictionary<string, string> ExtraireParametre(string args)
        {
            Dictionary<string, string> parametres = new();

            if (_regexParam().IsMatch(args))
            {
                _regexParam().Match(args).Groups["param"].Captures.ToList().ForEach(group =>
                {
                    string paramName = _regexParam().Match(group.Value).Groups["paramName"].Value;
                    string paramValue = _regexParam().Match(group.Value).Groups["paramValue"].Value;

                    parametres.Add(paramName, paramValue);
                });
            }

            return parametres;
        }

        /// <summary>
        /// Transformer un string d'ids fournit en parametre en liste d'ids
        /// ex: 1-3;5;7-9 => [1, 2, 3, 5, 7, 8, 9]
        ///     1;2;3 => [1, 2, 3]
        ///     4-6 => [4, 5, 6]
        /// </summary>
        /// <param name="args">String d'ids</param>
        private static List<int> ArgIdsToList(string args)
        {
            List<int> ListeIds = [];

            string argsId = _regexIds().Match(args).Groups["ids"].Value;

            _regexIds().Match(args).Groups["ids"].Captures.ToList().ForEach(groupId =>
            {
                if (groupId.Value.Contains('-'))
                {
                    string[] range = groupId.Value.Split('-');
                    if (range.Length == 2 && int.TryParse(range[0], out int start) && int.TryParse(range[1], out int end))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            if (!ListeIds.Contains(i))
                            {
                                ListeIds.Add(i);
                            }
                        }
                    }

                }
                else
                {
                    if (int.TryParse(groupId.Value, out int i) && !ListeIds.Contains(i))
                    {
                        ListeIds.Add(i);
                    }
                }
            });

            return ListeIds;
        }

        /// <summary>
        /// Lancer une sauvegarde
        /// </summary>
        /// <param name="args">Arguments de la commande contenant un string d'id de sauvegarde</param>
        private static void LancerSauvegarde(string args)
        {
            List<int> ListeIds = [];

            if (_regexIds().IsMatch(args))
            {
                ListeIds = ArgIdsToList(args);
            }
            else if (_regexParam().IsMatch(args))
            {
                /* Si l'argument "all" est present, lancer toutes les sauvegardes */

                if (ExtraireParametre(args).ContainsKey("all"))
                {
                    ListeIds = _saveConfiguration.GetConfiguration().ConvertAll(save => save.Id);
                }
            }

            _easySave.LancerSauvegarde(ListeIds);
        }

        /// <summary>
        /// Liste les sauvegardes enregistrees (configuration + id)
        /// </summary>
        private static void ListerSauvegardes()
        {
            _saveConfiguration.GetConfiguration().ForEach(save =>
            {
                Console.WriteLine(save);
            });
        }

        /// <summary>
        /// Ajouter une configuration de sauvegarde
        /// </summary>
        /// <param name="args">Arguments de la commande</param>
        /// <exception cref="ArgumentException">Il manque des parametres</exception>""
        private static void AjouterConfiguration(string args)
        {
            if (_regexParam().IsMatch(args))
            {
                Dictionary<string, string> parametres = ExtraireParametre(args);

                string name = parametres.GetValueOrDefault("name") ?? parametres.GetValueOrDefault("n") ?? throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter") + " : name");
                string inputFolder = parametres.GetValueOrDefault("inputFolder") ?? parametres.GetValueOrDefault("i") ?? throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter") + " : input");
                string outputFolder = parametres.GetValueOrDefault("outputFolder") ?? parametres.GetValueOrDefault("o") ?? throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter") + " : output");
                SaveType saveType = parametres.GetValueOrDefault("type") != null ? (SaveType)Enum.Parse(typeof(SaveType), parametres.GetValueOrDefault("type")) : SaveType.COMPLETE;

                _saveConfiguration.AddConfiguration(name, inputFolder, outputFolder, saveType);
            }
            else
            {
                throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter"));
            }
        }

        /// <summary>
        /// Modifier une configuration de sauvegarde
        /// </summary>
        /// <param name="args">Arguments de la commande</param>
        /// <exception cref="ArgumentException">Il manque des parametres</exception>"
        private static void ModifierConfiguration(string args)
        {
            if (_regexParam().IsMatch(args))
            {
                Dictionary<string, string> parametres = ExtraireParametre(args);

                int id = int.Parse(parametres.GetValueOrDefault("id") ?? throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter") + " : id"));

                _saveConfiguration.UpdateConfiguration(id, parametres.GetValueOrDefault("name"), parametres.GetValueOrDefault("inputFolder"), parametres.GetValueOrDefault("outputFolder"), (SaveType)Enum.Parse(typeof(SaveType), parametres.GetValueOrDefault("type")));
            }
            else
            {
                throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter"));
            }
        }

        /// <summary>
        /// Supprimer une configuration de sauvegarde
        /// </summary>
        /// <param name="args">Arguments de la commande</param>
        /// <exception cref="ArgumentException">L'id de la sauvegarde a supprimer n'est pas renseignee</exception>"
        private static void SupprimerConfiguration(string args)
        {
            if (_regexIds().IsMatch(args))
            {
                string argsId = _regexIds().Match(args).Groups["ids"].Value;

                _regexIds().Match(args).Groups["ids"].Captures.ToList().ForEach(groupId =>
                {
                    ArgIdsToList(args).ForEach(id =>
                    {
                        _saveConfiguration.RemoveConfiguration(id);
                    });
                });
            }
            else
            {
                throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter") + " id");
            }
        }

        /// <summary>
        /// Changer la langue de l'application
        /// </summary>
        /// <param name="args">Arguments de la commande</param>
        /// <exception cref="ArgumentException">La langue n'est pas renseignee</exception>""
        private static void ChangerLangue(string args)
        {
            if (args != null)
            {
                SharedLocalizer.SetCulture(args);
            }
            else
            {
                throw new ArgumentException(SharedLocalizer.GetLocalizedString("MissingParameter") + " : lang");
            }
        }

        /// <summary>
        /// Afficher l'aide
        /// </summary>
        private static void AfficherAide()
        {
            Console.WriteLine(SharedLocalizer.GetLocalizedString("LaunchSave"));
            Console.WriteLine();
            Console.WriteLine(SharedLocalizer.GetLocalizedString("ListSave"));
            Console.WriteLine();
            Console.WriteLine(SharedLocalizer.GetLocalizedString("CreateSave"));
            Console.WriteLine();
            Console.WriteLine(SharedLocalizer.GetLocalizedString("UpdateSave"));
            Console.WriteLine();
            Console.WriteLine(SharedLocalizer.GetLocalizedString("RemoveSave"));
            Console.WriteLine();
            Console.WriteLine(SharedLocalizer.GetLocalizedString("Help"));
            Console.WriteLine();
            Console.WriteLine(SharedLocalizer.GetLocalizedString("ClearConsole"));
            Console.WriteLine();
            Console.WriteLine(SharedLocalizer.GetLocalizedString("Language"));
        }
    }
}
