import {  Injectable    } from '@angular/core';
import {  ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  constructor(private toastr: ToastrService) { }

  showSuccessMessage(title: string, body: string) {
    this.toastr.success(body, title, {
      timeOut: 5000,
      closeButton: true,
      positionClass: 'toast-top-right',
      enableHtml: true,
      tapToDismiss: true,
    })
  }

  showErrorMessage(title: string, body: string) {
    this.toastr.error(body, title, {
      timeOut: 5000,
      closeButton: true,
      positionClass: 'toast-top-right',
      enableHtml: true,
      tapToDismiss: false,
    })
  }
}
