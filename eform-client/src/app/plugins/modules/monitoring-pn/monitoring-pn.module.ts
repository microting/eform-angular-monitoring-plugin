import { NgModule } from '@angular/core';
import {CommonModule, registerLocaleData} from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MDBBootstrapModule } from 'port/angular-bootstrap-md';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedPnModule } from '../shared/shared-pn.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { EformSharedModule } from '../../../common/modules/eform-shared/eform-shared.module';
import {MonitoringPnLayoutComponent} from './layouts';
import {MonitoringPnRoutingModule} from './monitoring-pn.routing.module';
import {CalendarModule, DateAdapter} from 'angular-calendar';
import {adapterFactory} from 'angular-calendar/date-adapters/date-fns';
import {OwlDateTimeModule} from 'ng-pick-datetime';
import {OwlMomentDateTimeModule} from 'ng-pick-datetime-moment';
import localeDa from '@angular/common/locales/da';
import {CasesModule} from '../../../modules';
import {MonitoringPnNotificationRulesService, MonitoringPnSettingsService} from './services';
import {
  NotificationRulesCreateComponent
} from './components/notification-rules/notification-rules-create/notification-rules-create.component';
import {
  NotificationRulesDeleteComponent,
  NotificationRulesEditComponent,
  NotificationRulesPageComponent
} from './components/notification-rules';

registerLocaleData(localeDa);

@NgModule({
  imports: [
    CommonModule,
    SharedPnModule,
    MDBBootstrapModule,
    MonitoringPnRoutingModule,
    TranslateModule,
    FormsModule,
    NgSelectModule,
    EformSharedModule,
    FontAwesomeModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    OwlDateTimeModule,
    OwlMomentDateTimeModule,
    CasesModule
  ],
  declarations: [
    MonitoringPnLayoutComponent,
    NotificationRulesPageComponent,
    NotificationRulesCreateComponent,
    NotificationRulesEditComponent,
    NotificationRulesDeleteComponent
  ],
  providers: [MonitoringPnSettingsService, MonitoringPnNotificationRulesService]
})

export class MonitoringPnModule { }
