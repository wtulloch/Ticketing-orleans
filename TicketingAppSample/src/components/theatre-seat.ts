import {LitElement, TemplateResult, customElement, html, CSSResult, css, property} from 'lit-element';
import { ITicketStatus } from '../models/show-info.model';

@customElement('theatre-seat')
export class TheatreSeat extends LitElement {

    @property({type: Object,attribute:'ticket'})
    ticket: ITicketStatus = {
        ticketId: '',
        sold: false
    };

    static get styles(): CSSResult { 
        return css `
        .ticket-card {
            width: 100px;
            height: 100px;
            display: flex;
            justify-content: center;
            align-content: center;
            border: 1px solid black;
            margin: 5px;
            background-color: #3cd63c;
            opacity: 0.8;
        }

        .ticket-card:hover {
            cursor: pointer;
            opacity: 1;
        }

        .ticket-sold {
            background-color: red;
        }
        `;
    }

    render(): TemplateResult {
        return this.getSeatDisplay();
      
        }
        private getDisplayNumber(ticketId: string) : string {
           if(!ticketId) {
               return '';
           }
            const regex: RegExp = /-(\d+)$/;
            let matches = ticketId.match(regex);
            if (matches) {
                return matches[1];
            }

            return '';
        }
        private getSeatDisplay()  {
            if (this.ticket.sold) {
                return html `<div class='ticket-card ticket-sold'>
                <p>${this.getDisplayNumber(this.ticket.ticketId)}</p>
            </div> `;
            }else {
               return html `<div class='ticket-card'>
                <p>${this.getDisplayNumber(this.ticket.ticketId)}</p>
            </div>`
            }
        }
    
}