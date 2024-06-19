import {    Component       } from '@angular/core';
import {    CommonModule    } from '@angular/common';
import {    MatDialogModule } from "@angular/material/dialog";
import {    FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {    MatFormFieldModule  } from "@angular/material/form-field";
import {    MatInputModule  } from "@angular/material/input";
import {    RouterLink        } from "@angular/router";
import {    AuthService       } from "../../../../services/auth.service";
import {    EmployeeService   } from "../../../../services/employee.service";
import {    MessagesService   } from "../../../../services/messages.service";
import {    InsertEmployee    } from 'src/app/models/employeeinsert';


@Component({
  selector: 'app-create-employee',
  standalone: true,
  imports: [CommonModule, MatDialogModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, RouterLink],
  templateUrl: './create-employee.component.html',
  styleUrls: ['./create-employee.component.css']
})
export class CreateEmployeeComponent {
  createEmployee: FormGroup;

  constructor(
    private fb: FormBuilder, private authService: AuthService, private employeeService: EmployeeService,
    private messagesService: MessagesService
  ) {
    this.createEmployee = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).{8,}$'), Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).{8,}$'), Validators.minLength(8)]],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]]
    }, { validators: this.passwordConfirmValidator })
  }

  passwordConfirmValidator(form: FormGroup) {
    const passwordControl = form.get('password');
    const confirmPasswordControl = form.get('confirmPassword');
  
    if (!passwordControl || !confirmPasswordControl) {
      return {};
    }
  
    if (passwordControl.value !== confirmPasswordControl.value) {
      confirmPasswordControl.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      if (confirmPasswordControl.hasError('passwordMismatch')) {
        confirmPasswordControl.setErrors(null);
      }
    }
    return {};
  }

  saveEmployee() {
    const employee: InsertEmployee = {
      username: this.createEmployee.controls['username'].value,
      password: this.createEmployee.controls['password'].value,
      firstname: this.createEmployee.controls['firstname'].value,
      lastname: this.createEmployee.controls['lastname'].value,
      email: this.createEmployee.controls['email'].value,
      phoneNumber: this.createEmployee.controls['phoneNumber'].value
    }

    this.employeeService.insertEmployee(employee).subscribe({
      complete: () => {},
      next: () => {
        this.messagesService.showSuccessMessage("Success!", 'Employee inserted successfully!')
        this.createEmployee.reset()
        this.createEmployee.markAsUntouched()
        this.createEmployee.markAsPristine()
      },
      error: (error) => this.messagesService.showErrorMessage('Error: ' + error.status, error.message)
    })
  }
}
