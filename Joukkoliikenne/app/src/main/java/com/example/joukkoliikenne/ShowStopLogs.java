package com.example.joukkoliikenne;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.method.ScrollingMovementMethod;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;

import java.util.ArrayList;

public class ShowStopLogs extends AppCompatActivity {

    TextView tv;
    ArrayList<String> lista_log;
    String oletus;
    Button button;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_show_stop_logs);

        oletus = "Tallennettuja logitietoja ei vielä ole!";
        lista_log = new ArrayList<>();

        tv = findViewById(R.id.textView5);
        tv.setMovementMethod(new ScrollingMovementMethod());
        button = findViewById(R.id.button3);

        Intent i = getIntent();
        lista_log = i.getStringArrayListExtra("logit");

        showList();

        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                lista_log.clear();
                System.out.println("LISTAN SISÄLTÖ: " + lista_log);

                Intent intentClear= new Intent();
                intentClear.putExtra("uudet_logit", lista_log); // listan sisältö päivittyy pääikkunaan
                setResult(RESULT_OK, intentClear);

                tv.setText(oletus);
            }
        });
    }

    private void showList(){
        if(lista_log.size() != 0){  // jos lista on tyhjä niin kerrotaan käyttäjälle, että logitietoja ei ole vielä olemassa
            tv.setText(""); // tyhjennetään oletus näkymä
            int size = lista_log.size() - 1;
            //System.out.println("LISTAN SISÄLTÖ: " + lista_log);
            for (int j = size; j >= 0 ; j--) {    // näytetään uusin logitieto ekana
                tv.setText(tv.getText() + lista_log.get(j) + "\n\n"); // logitiedot näkyviin textviewille omilla riveillä
            }
        }
    }
}
