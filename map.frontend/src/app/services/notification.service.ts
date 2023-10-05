import { Injectable } from "@angular/core";
import Swal from "sweetalert2";

@Injectable({
    providedIn: 'root',
})
export class NotificationService {
    constructor() {
    }
    alertSussess(message: string) {
        Swal.fire({
            toast: true,
            icon: 'success',
            position: 'top-end',
            title: message,
            showConfirmButton: false,
            showCloseButton: true,
            timer: 10000,
            timerProgressBar: true,
        })
    }
    alertError(message: string) {
        Swal.fire({
            toast: true,
            icon: 'error',
            position: 'top-end',
            title: message,
            showConfirmButton: false,
            showCloseButton: true,
            timer: 10000,
            timerProgressBar: true,
        })
    }
    alertErrorResponeAbp(err: any) {
        var message: string = '';
        if (err.error == null) {
            if (err.status != null || err.statusText != null)
                message = '[' + err.status + '] - ' + err.statusText;
            else
                message = err;
        }
        else if (err.error.Error == null)
            message = err.error;
        else
            message = (err.error.Error.Details || err.error.Error.Message);
        Swal.fire({
            toast: true,
            icon: 'error',
            position: 'top-end',
            title: message,
            showConfirmButton: false,
            showCloseButton: true,
            timer: 10000,
            timerProgressBar: true,
        })
    }
    alertWarning(message: string) {
        Swal.fire({
            toast: true,
            icon: 'warning',
            position: 'top-end',
            title: message,
            showConfirmButton: false,
            showCloseButton: true,
            timer: 10000,
            timerProgressBar: true,
        })
    }
    alertInfor(message: string) {
        Swal.fire({
            toast: true,
            icon: 'info',
            position: 'top-end',
            title: message,
            showConfirmButton: false,
            showCloseButton: true,
            timer: 10000,
            timerProgressBar: true,
        })
    }
    confirm(text: string): boolean {
        var response = false;
        Swal.fire({
            title: text,
            showConfirmButton: true,
            showCancelButton: true,
            confirmButtonText: 'OK',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                response = true;
            }
        })
        return response;
    }
}