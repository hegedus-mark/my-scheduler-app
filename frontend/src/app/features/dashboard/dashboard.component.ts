import {ChangeDetectionStrategy, Component} from '@angular/core';
import {Calendar, CheckSquare, Clock, Users, LucideAngularModule} from "lucide-angular";

@Component({
  selector: 'app-dashboard',
  imports: [LucideAngularModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent {
  readonly Calendar = Calendar;
  readonly CheckSquare = CheckSquare;
  readonly Clock = Clock;
  readonly Users = Users;
}
