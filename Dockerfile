FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG VERSION
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY / ./
RUN dotnet publish Rstolsmark.WakeOnLanServer.csproj -c Release -p:Version=$VERSION -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
VOLUME /app/data
ENTRYPOINT ["dotnet", "Rstolsmark.WakeOnLanServer.dll"]
# Run with:
# docker run -d \
# -p 80:8080 \
# -e PasswordAuthenticationOptions__HashedPassword=AQAAAAEAACcQAAAAEKf2sSufN9V6t+tENnq9sAE/IOhmb1PQzTAOdLXcUj3PPJuVkx6Ku/2De4jb1NCdeA== \
# -e PasswordAuthenticationOptions__Realm="Wake me up" \
# --name wakeonlanserver \
# --mount source=wakeonlanserver,target=/app/data \
# rstolsmark/wakeonlanserver
#
# Backup (backup is stored as backup.tar in current directory)
# docker run --rm --volumes-from wakeonlanserver -v $(pwd):/backup ubuntu tar cvf /backup/backup.tar /app/data
#
# Restore (assume backup.tar is stored in current directory)
# docker run --rm --volumes-from wakeonlanserver -v $(pwd):/backup ubuntu bash -c "cd /app/data && tar xvf /backup/backup.tar --strip 1"
