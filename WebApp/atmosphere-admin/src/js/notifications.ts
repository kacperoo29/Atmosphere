import * as bootstrap from 'bootstrap';

export function notify(title: string, msg: string) {
    var toast = build_toast(title, msg);
    var toast_container = document.getElementById('toast-container');
    if (toast_container === null) {
        toast_container = document.createElement('div');
        toast_container.id = 'toast-container';
        toast_container.classList.add('position-fixed', 'bottom-0', 'end-0', 'p-3');
        toast_container.style.zIndex = '9999';
        document.body.appendChild(toast_container);
    }

    toast_container.appendChild(toast);
    var toast_obj = new bootstrap.Toast(toast);
    toast_obj.show();
}

export function show_error(text: string) {
    notify('Error', text);
}

export function set_send_ping_interval(ws: WebSocket, interval: number) {
    setInterval(() => {
        ws.send('ping');
    }, interval);
}

export function set_on_close(ws: WebSocket) {
    window.onclose = () => {
        console.log('Closing websocket connection');
        ws.close();
    };
}

function build_toast(title: string, msg: string) {
    var toast = document.createElement('div');
    toast.classList.add('toast');
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    toast.innerHTML = `
        <div class="toast-header">
            <strong class="me-auto">${title}</strong>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
        <div class="toast-body">
            ${msg}
        </div>
    `;

    return toast;
}
