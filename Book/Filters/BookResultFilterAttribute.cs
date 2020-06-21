﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Filters
{
    public class BookResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, 
            ResultExecutionDelegate next)
        {
            ObjectResult resultFromAction = context.Result as ObjectResult;
            if(resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            if(typeof(System.Collections.IEnumerable).
                IsAssignableFrom(resultFromAction.Value.GetType()))
            {

            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            resultFromAction.Value = mapper.Map<Models.Book>(resultFromAction.Value);

            await next();
        }
    }
}