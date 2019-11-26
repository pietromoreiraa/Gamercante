using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamer.Models
{
    public class DataChart
    {

        public static Dictionary<object, object> SelecionaColunasGrafico()
        {
            Dictionary<object, object> dic = new Dictionary<object, object>();
            dic.Add("value", "X");
            dic.Add("number", "Amount");
            return dic;
        }
        public static Dictionary<object, object> SelecionaLinhasBanco()
        {
            Dictionary<object, object> dic = new Dictionary<object, object>();
            Gamer.Models.Context db = new Models.Context();

            var result2 = (from p in db.Transacoes

                           group p by p.Tday into g
                           select new
                           {
                               TransactionCode = g.Key,
                              
                               Quantidade = g.Count()
                           }).OrderBy(o => o.TransactionCode); ;

            var result = db.Transacoes.OrderBy(c => c.Tday).Select(gp => new {TQuantity = gp.TCount, TDia = gp.Tday });
            
            int Quantity = result.Sum(c => c.TQuantity);

            foreach (var item in result2)
            {
                dic.Add(item.TransactionCode, item.Quantidade);
            }


            return dic;
        }

        public static decimal SelecionaClassificacao(string GameId)
        {
           
            Gamer.Models.Context db = new Models.Context();

            int gameid = Convert.ToInt32(GameId);
            var rates = db.Rates.Where(c => c.GameId == gameid).Select(gp => new { Rates = gp.Rating });
            int resultcount = rates.Count();

            int resultTotal = rates.Sum(c => c.Rates);

            decimal medium = Convert.ToDecimal(resultTotal) / Convert.ToDecimal(resultcount);

            return medium;
        }

    }
}