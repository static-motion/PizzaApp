// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', () => {
    document.querySelector('.container-main').style.display = 'block';
});

document.addEventListener('DOMContentLoaded', () => {
    const dropdownIcon = document.querySelector(".user-icon-dropdown");
    const dropdownMenu = document.querySelector(".account-dropdown-menu")
    dropdownIcon.addEventListener('click', () => {
        dropdownMenu.classList.toggle('hidden');
    });
});