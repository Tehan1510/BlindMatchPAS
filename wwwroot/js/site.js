// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// =========================================
// PASSWORD TOGGLE - Login Page
// =========================================
function togglePassword() {
    const input = document.getElementById('passwordInput');
    const icon = document.getElementById('eyeIcon');
    if (!input || !icon) return;
    if (input.type === 'password') {
        input.type = 'text';
        icon.className = 'bi bi-eye-slash';
    } else {
        input.type = 'password';
        icon.className = 'bi bi-eye';
    }
}

// =========================================
// REGISTER PAGE - Combine split email inputs
// =========================================
document.addEventListener('DOMContentLoaded', function () {

    // --- Email assembly for Register form ---
    const registerForm = document.getElementById('registerForm');
    if (registerForm) {
        registerForm.addEventListener('submit', function (e) {
            const username = document.getElementById('UsernameInput');
            const domain = document.getElementById('DomainSelect');
            const finalEmail = document.getElementById('FinalEmail');

            if (username && domain && finalEmail) {
                const usernameVal = username.value.trim();
                const domainVal = domain.value;

                if (!usernameVal) {
                    e.preventDefault();
                    alert('Please enter your username before registering.');
                    return;
                }

                finalEmail.value = usernameVal + '@' + domainVal;
            }
        });
    }

    // --- Password mismatch check for Register form ---
    const passwordField = document.getElementById('passwordField');
    const confirmPasswordField = document.getElementById('confirmPasswordField');
    const mismatchDiv = document.getElementById('passwordMismatch');

    if (confirmPasswordField && passwordField && mismatchDiv) {
        confirmPasswordField.addEventListener('input', function () {
            if (confirmPasswordField.value !== passwordField.value) {
                mismatchDiv.classList.remove('d-none');
            } else {
                mismatchDiv.classList.add('d-none');
            }
        });
    }

});