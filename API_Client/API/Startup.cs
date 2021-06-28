using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Sys;
using Service.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SetSysConfigInfo();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("API", new OpenApiInfo { Title = "API Demo", Version = "v1" });

                #region 启用swagger验证功能
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，CoreAPI。
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, Array.Empty<string>() 
                    }
                });
                #endregion
            });
            
            services.AddAuthentication(options =>
            {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    //ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = SysConfigInfo.Audience,//Audience
                    ValidIssuer = SysConfigInfo.Issuer,//Issuer，这两项和签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SysConfigInfo.SecurityKey)),//拿到SecurityKey
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null ;
            });

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();


            //1.先开启认证
            app.UseAuthentication();
            //2.再开启授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/API/swagger.json", "API Demo v1");
            });

        }

        public void SetSysConfigInfo()
        {
            SysConfigInfo.SecurityKey = Configuration.GetValue<string>("Token:SecurityKey");
            SysConfigInfo.Issuer = Configuration.GetValue<string>("Token:Issuer");
            SysConfigInfo.Audience = Configuration.GetValue<string>("Token:Audience");
            SysConfigInfo.DBConnectionString = Encrypt.DESDecrypt(Configuration.GetValue<string>("DBConnectionString"));
            SysConfigInfo.UploadFilePath = AppDomain.CurrentDomain.BaseDirectory + Configuration.GetValue<string>("SysConfig:UploadFilePath");
        }
    }
}
