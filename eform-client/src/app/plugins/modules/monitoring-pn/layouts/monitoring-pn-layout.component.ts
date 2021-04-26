import {AfterViewInit, Component, OnInit} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {translates} from '../i18n/translates';
import {AuthStateService} from 'src/app/common/store';

@Component({
  selector: 'app-monitoring-pn-layout',
  template: '<router-outlet></router-outlet>',
})
export class MonitoringPnLayoutComponent implements AfterViewInit, OnInit {
  constructor(
    private translateService: TranslateService,
    private authStateService: AuthStateService
  ) {}

  ngOnInit(): void {}

  ngAfterViewInit() {
    setTimeout(() => {
      const lang = this.authStateService.currentUserLocale;
      const i18n = translates[lang];
      this.translateService.setTranslation(lang, i18n, true);
    }, 1000);
  }
}
