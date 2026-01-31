# GIBS Media Gallery Module for Oqtane

A flexible and modern media gallery module for the Oqtane Framework, designed to provide an easy way to manage and display albums of photos.

## Introduction

The GIBS Media Gallery is a custom module for the Oqtane Framework that allows administrators to create and manage photo albums. It features a clean, responsive, Piwigo-inspired layout for displaying albums and a lightbox for viewing individual photos.

## Features

- **Album Management**: Create, edit, and delete photo albums / categories.
- **Photo Management**: Upload photos, assign them to albums, and manage their details (title, description, etc.).
- **Automatic Title Generation**: Automatically creates a clean title from the filename on upload.
- **Thumbnail Generation**: Automatically creates thumbnails for uploaded images.
- **Interactive Album View**: A responsive grid layout for albums, inspired by Piwigo.
- **Photo Grid View**: Displays all photos within an album in a responsive grid.
- **Lightbox Viewer**: Click on a photo to view it in a full-screen lightbox with next/previous navigation.
- **Role-Based Security**: Control which user roles can view specific albums.
- **Oqtane Integration**: Fully integrated with Oqtane's module architecture, security, and file management system.

## Installation

1.  **Build the Solution**: Compile the `GIBS.Module.MediaGallery.sln` solution in **Release** mode.
2.  **Locate the NuGet Package**: Find the generated NuGet package in the `Package` folder of the solution directory.
3.  **Install in Oqtane**:
    - Log in to your Oqtane instance as a Host user.
    - Navigate to the **Admin Dashboard**.
    - Go to **Module Management**.
    - Click **Install Module** and upload the `GIBS.Module.MediaGallery.x.x.x.nupkg` file.

## Usage

1.  **Add Module to a Page**:
    - Navigate to the page where you want to display the gallery.
    - Click the **Edit** button (pencil icon) to enter edit mode.
    - From the module list, drag and drop the **Media Gallery** module onto a pane.

2.  **Configure Settings**:
    - Click the **Manage Settings** icon on the module wrapper.
    - **File Folder**: Select the folder where you want to store the uploaded media files.
    - Click **Save**.

3.  **Manage the Gallery**:
    - Click the **Manage Gallery** link that appears on the module (visible to users with Edit permissions).
    - **Albums Tab**: Create new albums (categories) for your photos.
    - **Photos Tab**: Click **Add Photo** to upload new images. The file uploader will appear.
    - After uploading a file, you can assign it to an album, edit its title, and save.

## For Developers

### Project Structure

-   **Client**: Contains the Blazor components (`.razor` files) that make up the module's user interface, as well as the client-side service for communicating with the server.
-   **Server**: Contains the API controllers, server-side services (business logic), repository for data access, and database migrations.
-   **Shared**: Contains the data models used by both the client and server projects.

### Key Dependencies

-   **Oqtane Framework**: The foundation of the module.
-   **SixLabors.ImageSharp**: Used on the server for generating image thumbnails.

---

This README provides a general overview. For more detailed information on Oqtane module development, please refer to the official [Oqtane Framework documentation](https://www.oqtane.org/).
