import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";


export const matchpassword: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  let password = control.get('newPassword')
  let passwordRepeat = control.get('passwordRepeat')
  if (password && passwordRepeat && (password?.value != passwordRepeat?.value) ) {
    return {
      passwordmatcherror: true
    }
  }
  return null;
};
