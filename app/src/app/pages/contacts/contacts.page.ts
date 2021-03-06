import { Component, OnInit } from '@angular/core';
import {Injectable} from '@angular/core';
import { ContactsLoaderService } from 'src/app/services/httpServices/contacts-loader.service';
import { ContactItem } from 'src/app/models/ContactItem';
import {ContactDetailsPage} from 'src/app/pages/contact-details/contact-details.page';
import { NavController } from '@ionic/angular';
import {Router} from '@angular/router';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.page.html',
  styleUrls: ['./contacts.page.css'],
})

export class ContactsPage implements OnInit {

  contactList: ContactItem[];

  groupedList: any[];

  constructor(
    private contactLoader: ContactsLoaderService,
    public router: Router
  ) {}

  ngOnInit() {
    this.contactLoader.getAllContacts().subscribe(data => {
      console.log(data);
      this.contactList = data;


      this.groupedList = new Array();
      const groups = Array.from(new Set(this.contactList.map(x => x.lastName[0])));

      groups.forEach(letter => {
        this.groupedList.push({
          letter: letter,
          contacts: this.contactList.filter(x => x.lastName[0] === letter)
        })
      });

    }, error => {console.error(error); });

    this.contactList.sort((a,b) => (a.lastName > b.lastName) ? 1 : -1)
  }

   /**
   * directs to contact-details.page
   */
  goToDetails(id: number) {
    console.log(id);
    this.router.navigate(['tabs', 'contacts', id.toString()]);
  }
}
