#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish "Theatre/Theater.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y libgdiplus

ENTRYPOINT ["dotnet", "Theater.dll"]