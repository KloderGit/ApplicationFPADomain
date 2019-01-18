using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiTests
{
    [TestClass]
    public class BuildTree
    {
        [TestMethod]
        public void Build()
        {
            var str = @"leads[status][0][id]=10701093&leads[status][0][name]=Семинар:+Пропитание+в+фитнесе+на+результат&leads[status][0][status_id]=142&leads[status][0][old_status_id]=20067736&leads[status][0][price]=22500&leads[status][0][responsible_user_id]=2079676&leads[status][0][last_modified]=1539782664&leads[status][0][modified_user_id]=2079718&leads[status][0][created_user_id]=2076025&leads[status][0][date_create]=1539782209&leads[status][0][pipeline_id]=9217056&leads[status][0][tags][0][id]=80147&leads[status][0][tags][0][name]=повторное+обращение&leads[status][0][account_id]=17769199&leads[status][0][custom_fields][0][id]=66339&leads[status][0][custom_fields][0][name]=Источник&leads[status][0][custom_fields][0][values][0][value]=Сайт&leads[status][0][custom_fields][0][values][0][enum]=139517&leads[status][0][custom_fields][1][id]=66349&leads[status][0][custom_fields][1][name]=Интересующая+услуга&leads[status][0][custom_fields][1][values][0][value]=Пропитание+в+фитнесе+на+результат&leads[status][0][custom_fields][1][values][0][enum]=153667&leads[status][0][custom_fields][2][id]=72333&leads[status][0][custom_fields][2][name]=Дата+проведения&leads[status][0][custom_fields][2][values][0]=1542067200&leads[status][0][custom_fields][3][id]=497267&leads[status][0][custom_fields][3][name]=Заявка+оплачена&leads[status][0][custom_fields][3][values][0][value]=1&leads[status][0][custom_fields][4][id]=566027&leads[status][0][custom_fields][4][name]=Подтвердил+участие&leads[status][0][custom_fields][4][values][0][value]=1&leads[status][0][custom_fields][5][id]=549619&leads[status][0][custom_fields][5][name]=Отправлено+СМС&leads[status][0][custom_fields][5][values][0][value]=1&leads[status][0][custom_fields][6][id]=566891&leads[status][0][custom_fields][6][name]=Анкета+участия&leads[status][0][custom_fields][6][values][0][value]=https://forms.amocrm.ru/tvzdtx?dp=Q1zaSQHqO%2BhHArUG1UMtpby%2FFHdWZSZgNIHZ%2BvJ%2FRAgB60%2FhsT%2F4Zwi%2F4qY37M3f&leads[status][0][custom_fields][7][id]=570933&leads[status][0][custom_fields][7][name]=Guid-Service&leads[status][0][custom_fields][7][values][0][value]=ab4b290b-9f78-11e6-80e7-0cc47a4b75cc&account[subdomain]=apfitness";

            var trre = str.Split('&');

            var arra = new List<List<string>>();

            foreach (var e in trre)
            {
                var ar = new List<string>( e.Split("[").Select(x => x.Replace("]", "")).ToArray());
                arra.Add(ar);
            }

            

            var Doop = Builder(arra).ToString();

            ;


        }


        JContainer Builder(IEnumerable<IEnumerable<string>> arrays)
        {
            JContainer result;

            var ttt = Wrap(arrays);

            result = new JObject();

            foreach (var name in ttt)
            {
                if (name.Contains("="))
                {
                    var value = name.Split('=');
                    result.Merge(new JObject(new JProperty(value[0], value[1])));
                }
                else
                {
                    var rr = Grouping(name, arrays);
                    var ss = Cut(rr);
                    var prop = new JProperty(name, Builder(ss));
                    result.Merge(new JObject(prop));
                }               
            }

            return result;
        }


        IEnumerable<string> Wrap(IEnumerable<IEnumerable<string>> arrays)
        {
            return arrays.Select(arr => {
                if (arr.ElementAtOrDefault(0) != null) return arr.ElementAt(0);
                else return null;
            })
            .Distinct()
            .Where(i => i != null)
            .ToList();
        }

        IEnumerable<IEnumerable<string>> Grouping(string name, IEnumerable<IEnumerable<string>> arrays)
        {
            var result = arrays.Where(arr => arr.ElementAt(0) == name);
            return result;
        }

        IEnumerable<IEnumerable<string>> Cut(IEnumerable<IEnumerable<string>> arrays)
        {
            var res = new List<List<string>>();

            foreach (var array in arrays)
            {
                var arr = new Stack<string>(array.Reverse());
                if (arr.Count > 0) arr.Pop();
                res.Add(arr.ToList<string>());
            }

            return res;
        }

















        public Dictionary<int, List<string>> asdasd(List<Stack<string>> arrays)
        {
            var longest = arrays.Select(arr=>arr.Count()).Max();

            var res = new Dictionary<int, List<string>>();

            for (var indx = 0; indx < longest; indx++)
            {
                var aaaa = arrays.Select(arr => {
                    if (arr.ElementAtOrDefault(indx) != null) return arr.ElementAt(indx);
                    else return null;
                })
                .Distinct()
                .Where(i => i != null)
                .ToList();

                res.Add(indx, new List<string>(aaaa));
            }

            return res;
        }


        public Dictionary<int, List<string>> retert(List<Stack<string>> arrays)
        {
            var longest = arrays.Select(arr => arr.Count()).Max();

            var res = new Dictionary<int, List<string>>();

            for (var indx = 0; indx < longest; indx++)
            {
                var aaaa = arrays.Select(arr => {
                    if (arr.ElementAtOrDefault(indx) != null) return arr.ElementAt(indx);
                    else return null;
                })
                .Distinct()
                .Where(i => i != null)
                .ToList();


            }

            return res;
        }


        IEnumerable<Stack<string>> Grouping(int indx, string name, IEnumerable<Stack<string>> arrays)
        {
            Stack<string> array3 = new Stack<string>(new[] { "account", "update", "0", "custom" }.Reverse());

            var result = arrays.Where(arr => arr.ElementAt(0) == name);
            return result;
        }




    }



    class Tree
    {
        public Node root = new Node();

        public void Build(Stack<string> array)
        {
            root = Recurs(array);
        }

        Node Recurs(Stack<string> array)
        {
            if (array.Count == 0) return new Node { Value = null };
            return new Node { Value = array.Pop(), Children = new List<Node> { Recurs(array) } };
        }


    }

    class Node
    {
        public string Value { get; set; }

        public IEnumerable<Node> Children { get; set; }
    }
}
