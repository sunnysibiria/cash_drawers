FROM docker-hub.binary.alfabank.ru/dotnet/core/aspnet:3.1

# http://confluence.moscow.alfaintra.net/pages/viewpage.action?pageId=370714778
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf

# set up network
ENV ASPNETCORE_URLS http://+:8080

WORKDIR /app
COPY ./src/CashManagment.Api/out/ ./

ENTRYPOINT ["dotnet", "CashManagment.Api.dll"]
