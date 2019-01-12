FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["../Clients/REST/RazorPages/RazorPages.csproj", "../Clients/REST/RazorPages/"]
RUN dotnet restore "../Clients/REST/RazorPages/RazorPages.csproj"
COPY . .
WORKDIR "/src/../Clients/REST/RazorPages"
RUN dotnet build "RazorPages.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RazorPages.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RazorPages.dll"]