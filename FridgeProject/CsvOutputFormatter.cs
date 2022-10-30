using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace FridgeProject
{
    public class CsvOutputFormatter : TextOutputFormatter {
        public CsvOutputFormatter() {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv")); 
            SupportedEncodings.Add(Encoding.UTF8); 
            SupportedEncodings.Add(Encoding.Unicode);
        }
        
        protected override bool CanWriteType(Type type) {
            if (typeof(FridgeDto).IsAssignableFrom(type) || typeof(IEnumerable<FridgeDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type); 
            }
            return false;
        }
        
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response; 
            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<FridgeDto>) {
                foreach (var fridgeDto in (IEnumerable<FridgeDto>)context.Object) {
                    FormatCsv(buffer, fridgeDto); 
                }
            }
            else
            {
                FormatCsv(buffer, (FridgeDto)context.Object);
            }
            await response.WriteAsync(buffer.ToString()); }
        
        private static void FormatCsv(StringBuilder buffer, FridgeDto fridgeDto) {
            buffer.AppendLine($"{fridgeDto.Id},\"{fridgeDto.Name},\"{fridgeDto.OwnerName},\"{fridgeDto.IdModel}\""); }
    }
}