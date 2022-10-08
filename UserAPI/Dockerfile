#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS sdkimage
WORKDIR /app

COPY ./UserAPI/*.csproj ./UserAPI/
COPY ./ServiceLayer/*.csproj ./ServiceLayer/
COPY ./DataAccess/*.csproj ./DataAccess/
COPY ./Entities/*.csproj ./Entities/

COPY ./*.sln .
RUN dotnet restore

COPY . .

RUN dotnet publish ./UserAPI/*.csproj -o /publish/


FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=sdkimage /publish .
ENV ASPNETCORE_URLS="http://*:1455"
ENTRYPOINT ["dotnet", "UserAPI.dll"]