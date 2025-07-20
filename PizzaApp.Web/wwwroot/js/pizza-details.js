document.addEventListener('DOMContentLoaded', addListeners);

function addListeners() {
    document.querySelector('.btn-back').addEventListener('click', () => {
        history.back()
    })
}