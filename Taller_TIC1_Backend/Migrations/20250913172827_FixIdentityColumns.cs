using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Taller_TIC1_Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoServicio",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoServicio", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    contacto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Repuesto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    numero_serie = table.Column<long>(type: "bigint", nullable: false),
                    precio = table.Column<float>(type: "real", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repuesto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Taller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    direccion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taller", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEmpleado",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEmpleado", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEstadoOrden",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEstadoOrden", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    placa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    marca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    modelo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    anio = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DetalleReparacionRepuesto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idRepuesto = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleReparacionRepuesto", x => x.id);
                    table.ForeignKey(
                        name: "FK_DetalleReparacionRepuesto_Repuesto_idRepuesto",
                        column: x => x.idRepuesto,
                        principalTable: "Repuesto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepuestoProveedor",
                columns: table => new
                {
                    idRepuesto = table.Column<int>(type: "integer", nullable: false),
                    idProveedor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepuestoProveedor", x => new { x.idRepuesto, x.idProveedor });
                    table.ForeignKey(
                        name: "FK_RepuestoProveedor_Proveedor_idProveedor",
                        column: x => x.idProveedor,
                        principalTable: "Proveedor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepuestoProveedor_Repuesto_idRepuesto",
                        column: x => x.idRepuesto,
                        principalTable: "Repuesto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    apellido = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    cedula = table.Column<long>(type: "bigint", nullable: false),
                    salario = table.Column<double>(type: "double precision", nullable: false),
                    fechaContratacion = table.Column<DateTime>(type: "date", nullable: false),
                    idTipoEmpleado = table.Column<int>(type: "integer", nullable: false),
                    TipoEmpleadoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.id);
                    table.ForeignKey(
                        name: "FK_Empleado_TipoEmpleado_TipoEmpleadoId",
                        column: x => x.TipoEmpleadoId,
                        principalTable: "TipoEmpleado",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Empleado_TipoEmpleado_idTipoEmpleado",
                        column: x => x.idTipoEmpleado,
                        principalTable: "TipoEmpleado",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdenTrabajo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaCreacion = table.Column<DateTime>(type: "date", nullable: false),
                    idTipoEstadoOrden = table.Column<int>(type: "integer", nullable: false),
                    TipoEstadoOrdenId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenTrabajo", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrdenTrabajo_TipoEstadoOrden_TipoEstadoOrdenId",
                        column: x => x.TipoEstadoOrdenId,
                        principalTable: "TipoEstadoOrden",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_OrdenTrabajo_TipoEstadoOrden_idTipoEstadoOrden",
                        column: x => x.idTipoEstadoOrden,
                        principalTable: "TipoEstadoOrden",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    telefono = table.Column<long>(type: "bigint", nullable: false),
                    idVehiculo = table.Column<int>(type: "integer", nullable: false),
                    VehiculoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cliente_Vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Cliente_Vehiculo_idVehiculo",
                        column: x => x.idVehiculo,
                        principalTable: "Vehiculo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VElectrico",
                columns: table => new
                {
                    idVehiculo = table.Column<int>(type: "integer", nullable: false),
                    capacidadBateria = table.Column<int>(type: "integer", nullable: false),
                    VehiculoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VElectrico", x => x.idVehiculo);
                    table.ForeignKey(
                        name: "FK_VElectrico_Vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_VElectrico_Vehiculo_idVehiculo",
                        column: x => x.idVehiculo,
                        principalTable: "Vehiculo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VGasolina",
                columns: table => new
                {
                    idVehiculo = table.Column<int>(type: "integer", nullable: false),
                    cilindraje = table.Column<int>(type: "integer", nullable: false),
                    VehiculoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VGasolina", x => x.idVehiculo);
                    table.ForeignKey(
                        name: "FK_VGasolina_Vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_VGasolina_Vehiculo_idVehiculo",
                        column: x => x.idVehiculo,
                        principalTable: "Vehiculo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VHibrido",
                columns: table => new
                {
                    idVehiculo = table.Column<int>(type: "integer", nullable: false),
                    capacidadBateria = table.Column<int>(type: "integer", nullable: false),
                    cilindraje = table.Column<int>(type: "integer", nullable: false),
                    VehiculoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VHibrido", x => x.idVehiculo);
                    table.ForeignKey(
                        name: "FK_VHibrido_Vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculo",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_VHibrido_Vehiculo_idVehiculo",
                        column: x => x.idVehiculo,
                        principalTable: "Vehiculo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosEmpleadoTaller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    contrasena = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    idTaller = table.Column<int>(type: "integer", nullable: false),
                    idEmpleado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosEmpleadoTaller", x => x.id);
                    table.ForeignKey(
                        name: "FK_UsuariosEmpleadoTaller_Empleado_idEmpleado",
                        column: x => x.idEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosEmpleadoTaller_Taller_idTaller",
                        column: x => x.idTaller,
                        principalTable: "Taller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CEmpresa",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "integer", nullable: false),
                    nit = table.Column<long>(type: "bigint", nullable: false),
                    representante = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEmpresa", x => x.idCliente);
                    table.ForeignKey(
                        name: "FK_CEmpresa_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CEmpresa_Cliente_idCliente",
                        column: x => x.idCliente,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CNatural",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "integer", nullable: false),
                    cedula = table.Column<long>(type: "bigint", nullable: false),
                    apellido = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CNatural", x => x.idCliente);
                    table.ForeignKey(
                        name: "FK_CNatural_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CNatural_Cliente_idCliente",
                        column: x => x.idCliente,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servicio",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idCliente = table.Column<int>(type: "integer", nullable: false),
                    idEmpleado = table.Column<int>(type: "integer", nullable: false),
                    idEstado = table.Column<int>(type: "integer", nullable: false),
                    costo = table.Column<float>(type: "real", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: true),
                    EmpleadoId = table.Column<int>(type: "integer", nullable: true),
                    EstadoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicio", x => x.id);
                    table.ForeignKey(
                        name: "FK_Servicio_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Servicio_Cliente_idCliente",
                        column: x => x.idCliente,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servicio_Empleado_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleado",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Servicio_Empleado_idEmpleado",
                        column: x => x.idEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servicio_EstadoServicio_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadoServicio",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Servicio_EstadoServicio_idEstado",
                        column: x => x.idEstado,
                        principalTable: "EstadoServicio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosClienteTaller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    contrasena = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    idTaller = table.Column<int>(type: "integer", nullable: false),
                    idCliente = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosClienteTaller", x => x.id);
                    table.ForeignKey(
                        name: "FK_UsuariosClienteTaller_Cliente_idCliente",
                        column: x => x.idCliente,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosClienteTaller_Taller_idTaller",
                        column: x => x.idTaller,
                        principalTable: "Taller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleReparacion",
                columns: table => new
                {
                    idServicio = table.Column<int>(type: "integer", nullable: false),
                    idDetalleReparacionRepuesto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleReparacion", x => x.idServicio);
                    table.ForeignKey(
                        name: "FK_DetalleReparacion_DetalleReparacionRepuesto_idDetalleRepara~",
                        column: x => x.idDetalleReparacionRepuesto,
                        principalTable: "DetalleReparacionRepuesto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleReparacion_Servicio_idServicio",
                        column: x => x.idServicio,
                        principalTable: "Servicio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleRevision",
                columns: table => new
                {
                    idServicio = table.Column<int>(type: "integer", nullable: false),
                    detalles = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleRevision", x => x.idServicio);
                    table.ForeignKey(
                        name: "FK_DetalleRevision_Servicio_idServicio",
                        column: x => x.idServicio,
                        principalTable: "Servicio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleServicioOrden",
                columns: table => new
                {
                    idOrdenTrabajo = table.Column<int>(type: "integer", nullable: false),
                    idServicio = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleServicioOrden", x => new { x.idOrdenTrabajo, x.idServicio });
                    table.ForeignKey(
                        name: "FK_DetalleServicioOrden_OrdenTrabajo_idOrdenTrabajo",
                        column: x => x.idOrdenTrabajo,
                        principalTable: "OrdenTrabajo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleServicioOrden_Servicio_idServicio",
                        column: x => x.idServicio,
                        principalTable: "Servicio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CEmpresa_ClienteId",
                table: "CEmpresa",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_idVehiculo",
                table: "Cliente",
                column: "idVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_VehiculoId",
                table: "Cliente",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_CNatural_ClienteId",
                table: "CNatural",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleReparacion_idDetalleReparacionRepuesto",
                table: "DetalleReparacion",
                column: "idDetalleReparacionRepuesto");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleReparacionRepuesto_idRepuesto",
                table: "DetalleReparacionRepuesto",
                column: "idRepuesto");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleServicioOrden_idServicio",
                table: "DetalleServicioOrden",
                column: "idServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_idTipoEmpleado",
                table: "Empleado",
                column: "idTipoEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_TipoEmpleadoId",
                table: "Empleado",
                column: "TipoEmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenTrabajo_idTipoEstadoOrden",
                table: "OrdenTrabajo",
                column: "idTipoEstadoOrden");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenTrabajo_TipoEstadoOrdenId",
                table: "OrdenTrabajo",
                column: "TipoEstadoOrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_RepuestoProveedor_idProveedor",
                table: "RepuestoProveedor",
                column: "idProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_ClienteId",
                table: "Servicio",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_EmpleadoId",
                table: "Servicio",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_EstadoId",
                table: "Servicio",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_idCliente",
                table: "Servicio",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_idEmpleado",
                table: "Servicio",
                column: "idEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Servicio_idEstado",
                table: "Servicio",
                column: "idEstado");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosClienteTaller_idCliente",
                table: "UsuariosClienteTaller",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosClienteTaller_idTaller",
                table: "UsuariosClienteTaller",
                column: "idTaller");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEmpleadoTaller_idEmpleado",
                table: "UsuariosEmpleadoTaller",
                column: "idEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEmpleadoTaller_idTaller",
                table: "UsuariosEmpleadoTaller",
                column: "idTaller");

            migrationBuilder.CreateIndex(
                name: "IX_VElectrico_VehiculoId",
                table: "VElectrico",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_VGasolina_VehiculoId",
                table: "VGasolina",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_VHibrido_VehiculoId",
                table: "VHibrido",
                column: "VehiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CEmpresa");

            migrationBuilder.DropTable(
                name: "CNatural");

            migrationBuilder.DropTable(
                name: "DetalleReparacion");

            migrationBuilder.DropTable(
                name: "DetalleRevision");

            migrationBuilder.DropTable(
                name: "DetalleServicioOrden");

            migrationBuilder.DropTable(
                name: "RepuestoProveedor");

            migrationBuilder.DropTable(
                name: "UsuariosClienteTaller");

            migrationBuilder.DropTable(
                name: "UsuariosEmpleadoTaller");

            migrationBuilder.DropTable(
                name: "VElectrico");

            migrationBuilder.DropTable(
                name: "VGasolina");

            migrationBuilder.DropTable(
                name: "VHibrido");

            migrationBuilder.DropTable(
                name: "DetalleReparacionRepuesto");

            migrationBuilder.DropTable(
                name: "OrdenTrabajo");

            migrationBuilder.DropTable(
                name: "Servicio");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "Taller");

            migrationBuilder.DropTable(
                name: "Repuesto");

            migrationBuilder.DropTable(
                name: "TipoEstadoOrden");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Empleado");

            migrationBuilder.DropTable(
                name: "EstadoServicio");

            migrationBuilder.DropTable(
                name: "Vehiculo");

            migrationBuilder.DropTable(
                name: "TipoEmpleado");
        }
    }
}
