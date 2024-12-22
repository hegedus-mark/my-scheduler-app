import { Component } from '@angular/core';
import {ButtonComponent} from "../../components/button/button.component";

@Component({
  selector: 'app-showcase',
  imports: [
    ButtonComponent
  ],
  templateUrl: './showcase.component.html',
  styleUrl: './showcase.component.scss'
})
export class ShowcaseComponent {

}
