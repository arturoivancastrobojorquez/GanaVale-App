# README - Sistema de Vales

## 🎯 Descripción Rápida

Aplicación de escritorio completa para gestionar vales y préstamos a clientes con Windows Forms y MySQL.

**Características:**
- Gestión de clientes
- Control de vales/préstamos
- Registro de pagos
- Cálculo automático de deudas
- Dashboard con totales
- Sistema de estados (Pagado, Atrasado, En tiempo)

## 📦 Dependencias

El proyecto utiliza la siguiente librería NuGet externa:
- **MySql.Data v8.2.0**: Conector de MySQL para C#

La librería se descargará automáticamente cuando compiles el proyecto.

## ⚡ Inicio Rápido

### 1. Requisitos
- Visual Studio 2022
- .NET 6.0 SDK
- MySQL Server (cualquier versión reciente)

### 2. Configuración
```
1. Abre Visual Studio
2. File → Open → Folder → Selecciona la carpeta del proyecto
3. Aguarda a que se cargue todo
4. Click derecho en SistemaVales.csproj → "Restore NuGet Packages"
```

### 3. Base de Datos
```
1. Abre MySQL Workbench
2. File → Open SQL Script → script_sql.sql
3. Execute (botón amarillo con rayo)
4. La BD se crea automáticamente
```

### 4. Conexión
```
1. Abre Data/ConexionBD.cs
2. Cambia las líneas 10-13 según tu MySQL:
   - usuario: "root" (normalmente)
   - contraseña: "" (vacía si no tienes)
   - baseDatos: "sistema_vales" (dejar igual)
```

### 5. Ejecutar
```
- Presiona F5 en Visual Studio
- O: dotnet run
```

## 📁 Estructura de Carpetas

```
├── Data/              → Acceso a base de datos
├── Models/            → Entidades (Cliente, Vale, Pago)
├── Business/          → Lógica de validación y negocio
├── Forms/             → Interfaz gráfica (Windows Forms)
├── script_sql.sql     → Script para crear BD
└── INSTRUCCIONES.md   → Guía detallada
```

## 🔑 Usuarios y Contraseñas

**MySQL (por defecto):**
- Usuario: `root`
- Contraseña: (vacía)

Al crear la BD se incluyen datos de prueba automáticamente.

## 🐛 Errores Comunes

| Problema | Solución |
|----------|----------|
| "No se pudo conectar a BD" | Verifica MySQL está ejecutándose y revisa ConexionBD.cs |
| "MySql.Data no encontrado" | Abre Package Manager Console y ejecuta: `Install-Package MySql.Data` |
| "Access denied for user" | Revisa usuario/contraseña en MySQL |
| "Base de datos no existe" | Ejecuta script_sql.sql en MySQL Workbench |

## 📖 Documentación

Abre `INSTRUCCIONES.md` para una guía completa paso a paso.

## 📝 Uso Básico

1. **Agregar Cliente:** Forms → Clientes → Nombre + Teléfono → Agregar
2. **Crear Vale:** Forms → Vales → Cliente + Monto + Fecha → Agregar
3. **Registrar Pago:** Forms → Pagos → Vale + Monto → Agregar
4. **Ver Dashboard:** Vuelve a la pantalla principal (se actualiza automáticamente)

## ✅ Lo que incluye

- ✅ Código completo y funcional
- ✅ Base de datos lista
- ✅ Interfaz gráfica intuitiva
- ✅ Validaciones de datos
- ✅ Cálculos automáticos
- ✅ Documentación detallada
- ✅ Datos de ejemplo
- ✅ Buenas prácticas (arquitectura en capas)

## 🚀 Próximos Pasos

- Agregar búsqueda por cliente
- Exportar reportes a Excel
- Sistema de usuarios y login
- Gráficos de estadísticas
- Alertas de vales próximos a vencer

## 📧 Soporte

Si tienes problemas:
1. Revisa `INSTRUCCIONES.md` (sección "Solución de Problemas")
2. Verifica la consola de errores en Visual Studio
3. Asegúrate que MySQL está ejecutándose
4. Revisa la cadena de conexión

---

**¡Listo para usar!** 🎉
