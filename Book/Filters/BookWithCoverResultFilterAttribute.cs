using AutoMapper;
using Books.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Filters
{
    public class BookWithCoverResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            ObjectResult resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var (book, bookCovers) = ((Entities.Book,IEnumerable<ExternalModels.BookCover>))resultFromAction.Value;

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            var mappedBook = mapper.Map<BookWithCovers>(book);
            resultFromAction.Value = mapper.Map(bookCovers, mappedBook);

            await next();
        }
    }
}
