using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace System.Web.Mvc
{
    public static class HtmlHelpers
    {

        public static string GraficoLine(this HtmlHelper html, string nomeGrafico, Dictionary<object, object> colunas, Dictionary<object, object> linhas,int width, int height)
        {
            StringBuilder sb = new StringBuilder();


            sb.AppendLine("<script language='javascript'>");
            sb.AppendLine("google.charts.load('current', { packages: ['corechart', 'line'] });");
            sb.AppendLine("google.charts.setOnLoadCallback(drawBasic);"); 

            sb.AppendLine("function drawBasic(){");

            sb.AppendLine("var data = new google.visualization.DataTable();");
            foreach (var coluna in colunas)
                sb.AppendLine(String.Format("data.addColumn('{0}', '{1}');", coluna.Key, coluna.Value));
            foreach (var linha in linhas)

                sb.AppendLine(String.Format("data.addRow(['{0}', {1}]);", linha.Key, linha.Value));

            sb.AppendLine("var options = { hAxis: { title: 'Dia' }, vAxis: { title: 'Vendas'}");
            sb.AppendLine(String.Format(" 'width': '{0}',", width));
            sb.AppendLine(String.Format(" 'height': '{0}' ", height));
            sb.AppendLine(" }; ");

            sb.AppendLine(String.Format("var chart = new google.visualization.LineChart(document.getElementById('{0}'));", nomeGrafico));

            sb.AppendLine("chart.draw(data, options);");
            
            sb.AppendLine("}");
            sb.AppendLine("</script>");
            return sb.ToString();

        }
    }
}