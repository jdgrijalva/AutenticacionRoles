# Sistema de Cartelera de Cine (Grupo 3)

## Problema Asignado
Un cine necesita administrar su cartelera en línea mediante una aplicación web con base de datos, validaciones, relación uno a muchos entre **Genero** (entidad principal) y **Pelicula** (entidad dependiente), autenticación con Identity y control de acceso basado en roles (**Curador** y **Espectador**).

## Integrantes y Turnos
* **Integrante 1 ([Joustin Grijalva ]):** Fase 1 y Fase 2 — Creación del proyecto, configuración de base de datos, entidad `Pelicula`, CRUD inicial y validaciones con `DataAnnotations` en español.
* **Integrante 2 ([William Villasis ]):** Fase 3 — Creación de la entidad principal `Genero`, relación uno a muchos (`GeneroId`), listas desplegables y visualización del género en las vistas.
* **Integrante 3 ([Cristina Guaman ]):** Fase 4 — Configuración de `Identity` con `ApplicationUser`, vistas parciales `_LoginPartial` y protección inicial de acceso.
* **Integrante 4 ([Wendy Bazan ]):** Fase 5 y Componente Autónomo — Creación automática de roles (`Curador` y `Espectador`) y usuarios de prueba, restricción por rol en controladores y vistas, y visualización del rol junto al correo en el menú.

## Credenciales de Prueba
* **Rol de Gestión (`Curador`):**
  * **Correo:** `curador@cine.com`
  * **Contraseña:** `Curador2026*`
* **Rol de Consulta (`Espectador`):**
  * **Correo:** `espectador@cine.com`
  * **Contraseña:** `Espectador2026*`

## Pasos para Ejecutar el Proyecto
1. Clonar el repositorio desde GitHub: `https://github.com`
2. Abrir la solución en Visual Studio y revisar en `appsettings.json` que la cadena de conexión apunte a `localhost\SQLEXPRESS`.
3. Abrir la **Consola del Administrador de paquetes** y ejecutar las migraciones en ambos contextos:
   - `Add-Migration -Context ApplicationContext`
   - `Add-Migration -Context AutenticacionRolesContext`
   - `Update-Database -Context ApplicationContext`
   - `Update-Database -Context AutenticacionRolesContext`
4. Ejecutar la aplicación con `F5`.
