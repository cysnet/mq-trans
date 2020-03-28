using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MQTransaction.Common.Models.Entity;
using MQTransaction.Common.Models.Requests;
using MQTransaction.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MQTransaction.Common.HttpMaps
{
    internal class MQtransHttpMaps
    {
        /// <summary>
        /// 回调地址
        /// </summary>
        /// <param name="app"></param>
        public static void CallbackMap(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var repository = context.RequestServices.GetService(typeof(IMQTransRepository)) as IMQTransRepository;

                var body = string.Empty;
                //启动倒带方式
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                if (!string.IsNullOrEmpty(body))
                {
                    var requestparmas = JsonConvert.DeserializeObject<CallBackRequest>(body);
                    var entity = repository.GetPublishMsgById(requestparmas.SourceMsgId);
                    if(entity.status != published_status.处理完成)
                    {
                        entity.status = published_status.处理完成;
                        repository.SaveChange();
                    }
                    await context.Response.WriteAsync("done");
                    return;
                }
                await context.Response.WriteAsync("err");
            });
        }

        /// <summary>
        /// 主动查询
        /// </summary>
        /// <param name="app"></param>
        public static void QueryStatusMap(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var repository = context.RequestServices.GetService(typeof(IMQTransRepository)) as IMQTransRepository;

                var body = string.Empty;
                //启动倒带方式
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                if (!string.IsNullOrEmpty(body))
                {
                    var requestparmas = JsonConvert.DeserializeObject<List<string>>(body);
                    var result = repository.GetReceiveMsgByIds(requestparmas);
                    context.Response.ContentType = "application/json; charset=utf-8";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }
            });
        }
    }
}
