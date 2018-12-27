FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Core.Services/Core.Services.csproj", "Core.Services/"]
COPY ["Core/Core.Application/Core.Application.csproj", "Core/Core.Application/"]
RUN dotnet restore "Core.Services/Core.Services.csproj"
COPY . .
WORKDIR "/src/Core.Services"
RUN dotnet build "Core.Services.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Core.Services.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Core.Services.dll"]