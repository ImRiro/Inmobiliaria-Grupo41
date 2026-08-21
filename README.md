# Inmobiliaria

## Integrantes:
- Ramiro Romero
- Nicolas Bustamante
## Diagramas

(Estado actual del proyecto, seran ampliados a medida que el scope crezca)

### Casos de Uso
https://www.figma.com/files/team/1614361052615337445/project/639110759?fuid=1614361051090803016

### Entidad Relacion
<img width="432" height="182" alt="Inmobiliaria Lab 2 - Romero Bustamante drawio" src="https://github.com/user-attachments/assets/6b16f230-4e8d-4660-9bb2-039ca02e53d3" />

### Instrucciones SQL

1. Abrir MySQL y conectarse al servidor local (localhost:3306).
2. Ejecutar el script "Inmobiliaria-lab2-RomeroBustamante.sql" incluido en este repositorio
    - Si estas usando DBeaver como es mi caso tenes que crear una nueva Base de Datos, darle segundo click, ir a tools -> Restore Database, ahi te abre una ventana y en el campo "input" pones el archivo.
    - En el caso de XAMPP tambien deberas crear una base de datos previamente y luego importar el archivo.
3. Verificar en "appsettings.json" que la cadena de conexión ("DefaultConnection") apunte al mismo usuario/contraseña (y base de datos si decidiste cambiarle el nombre) que usaste en tu instalación local de MySQL.
4. Y eso deberia ser suficiente, la base de datos deberia tener en si simplemente las tablas Inquilino y Propietario con 1 y 2 records de prueba respectivamente.
