using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GrammBOTv2.Services
{
    public class MessageHandler
    {
        List<int> numList = new List<int>();


        /// <summary>
        /// Обработка сообщения в случае, если сделан выбор операции
        /// </summary>
        /// <param name="OperationType"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Handle(string OperationType, string text){

            switch (OperationType){
                case "CountSing":
                    return $"Количество знаков в строке = {text.Length}";
                case "SumOfNumbers":
                    return $"{Calculate(text)}";
                default:
                    return "Сделайте ввод.";
            }
        }
        /// <summary>
        /// Вычисление суммы чисел, которые есть в строке
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Calculate(string text){
            string tempresString = "";
            numList.Clear();
            int i = 0;
            foreach (var item in text){
                i++;
                if (item != ' '){
                    if (char.IsDigit(item)){tempresString += item;}
                    else{return "Введите числа ' '. ";}

                    if (i == text.Length){numList.Add(int.Parse(tempresString));}
                }
                if (item == ' '){
                    numList.Add(int.Parse(tempresString));
                    tempresString = "";
                }
            }
            return "Сумма чисел = " + numList.Sum().ToString();
        }
    }
}
