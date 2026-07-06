#!/bin/bash

/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to be available..."
until /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P escalaApi34@FF -C -Q "SELECT 1" > /dev/null 2>&1; do
  sleep 1
  echo "Still waiting for SQL Server..."
done

echo "Running init.sql..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P escalaApi34@FF -C -i /docker-entrypoint-initdb.d/init.sql

MIGRATIONS_DIR="/docker-entrypoint-initdb.d/migrations"
if [ -d "$MIGRATIONS_DIR" ]; then
  for migration in $(ls "$MIGRATIONS_DIR"/*.sql 2>/dev/null | sort); do
    echo "Running migration: $(basename "$migration")..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P escalaApi34@FF -C -i "$migration"
  done
fi

echo "Database initialization complete."
wait
