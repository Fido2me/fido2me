#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0-jammy-chiseled AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /sln
COPY ./Fido2me.sln ./
COPY ["./src/Fido2me/Fido2me.csproj", "./src/Fido2me/Fido2me.csproj"]
COPY ["./src/Fido2/Fido2.csproj", "./src/Fido2/Fido2.csproj"]
COPY ["./src/Fido2.AspNet/Fido2.AspNet.csproj", "./src/Fido2.AspNet/Fido2.AspNet.csproj"]
COPY ["./src/Fido2.Models/Fido2.Models.csproj", "./src/Fido2.Models/Fido2.Models.csproj"]
COPY ["./test/Fido2Tests/Test.csproj", "./test/Fido2Tests/Test.csproj"]


RUN dotnet restore "./src/Fido2me/Fido2me.csproj"
COPY . .
WORKDIR "/sln/src"
RUN dotnet build "./Fido2me/Fido2me.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Fido2me/Fido2me.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fido2me.dll"]
