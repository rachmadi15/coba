$(document).ready(() => {
    $('.user-feedback').find('form').on('submit', (e) => {
        sessionStorage.setItem('skipFeedback', 1);
    });
});