Create database HoteleriaDB

use HoteleriaDB

-- Crear tabla Usuarios
CREATE TABLE Usuarios (
    id INT PRIMARY KEY identity(1, 1),
    nombre_usuario VARCHAR(50),
    contraseña VARCHAR(50)
);

-- Crear tabla Servicios_Hotel
CREATE TABLE Servicios_Hotel (
    id INT PRIMARY KEY identity(1, 1),
    calificacion FLOAT,
    descripcion VARCHAR(255),
    precio DECIMAL(10, 2),
    imagen VARCHAR(255)
);

-- Crear tabla Habitaciones
CREATE TABLE Habitaciones (
    id INT PRIMARY KEY identity(1, 1),
    calificacion FLOAT,
    nombre_habitacion VARCHAR(100),
    descripcion VARCHAR(255),
    precio DECIMAL(10, 2),
    imagen VARCHAR(255)
);

-- Crear tabla Contacto
CREATE TABLE Contacto (
    id INT PRIMARY KEY identity(1, 1),
    nombre VARCHAR(100),
    correo VARCHAR(100),
    numero_telefono VARCHAR(20),
    mensaje TEXT
);

CREATE TABLE Usuario_Servicios (
    id INT PRIMARY KEY identity(1, 1),
    id_usuario INT,
    id_servicio INT,
    calificacion FLOAT,
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id),
    FOREIGN KEY (id_servicio) REFERENCES Servicios_Hotel(id)
);

-- Crear tabla Calificaciones_Habitaciones
CREATE TABLE Usuario_Habitaciones (
    id INT PRIMARY KEY identity(1, 1),
    id_usuario INT,
    id_habitacion INT,
    calificacion FLOAT,
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id),
    FOREIGN KEY (id_habitacion) REFERENCES Habitaciones(id)
);

select * from Contacto



