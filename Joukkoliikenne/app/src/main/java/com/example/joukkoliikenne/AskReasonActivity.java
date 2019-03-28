package com.example.joukkoliikenne;

import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;

public class AskReasonActivity extends AppCompatActivity {

    SharedPreferences sharedPref;
    ListView lv;
    Button button;
    EditText et;

    ArrayList<String> lista;
    String choice = "";
    String addr = "";

    Gson gson;
    String json = "";
    Type collectionType;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ask_reason);

        Intent i = getIntent();
        addr = i.getStringExtra("osoite");

        sharedPref= PreferenceManager.getDefaultSharedPreferences(getApplicationContext());
        gson = new Gson();

        lv = findViewById(R.id.listView);
        button = findViewById(R.id.button);
        et = findViewById(R.id.editText);

        loadChoices();

        lv.setOnItemClickListener(new AdapterView.OnItemClickListener() {       // kun klikkaa jotain vaihtoehdoista niin tämä suoriutuu
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                Calendar calendar = Calendar.getInstance();
                String date = DateFormat.getDateInstance().format(calendar.getTime());

                SimpleDateFormat format = new SimpleDateFormat("HH:mm");
                String time = format.format(calendar.getTime());


                choice = lista.get(i);
                Intent intent = new Intent();
                String loggedStop = addr + " - " + choice + " (" + date + ", klo " + time + ")";
                intent.putExtra("loggedStop", loggedStop);

                // aseta paluuarvo
                setResult(RESULT_OK, intent);
                //suljetaan ikkuna ja palautetaan
                finish();
            }
        });

        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                lista.add(et.getText().toString()); // lisätään listaan uusi vaihtoehto editText fieldistä
                saveChoices();  // tallennetaan lista sharedPreferenceen
                showNewList();  // näytetään päivitetty lista
            }
        });
    }

    private void loadChoices(){
        //SharedPreferences sharedPreferences = getSharedPreferences("sharedPref");
        //json = sharedPref.getString()
        collectionType = new TypeToken<ArrayList<String>>() {}.getType();

        json = sharedPref.getString("choices", null);
        lista = gson.fromJson(json, collectionType); // tekee jsonista String listan

        if(lista == null){      // jos sharedPreferencestä ei saatu listalle sisältöä, niin tehdään oletuksena tällainen lista ja tallennetaan se
            lista = new ArrayList<>();
            lista.add("Liikennevalo");
            lista.add("Pysäkki");
            lista.add("Väistämisvelvollisuus");

            saveChoices();
        }

        showNewList();  // näytetään lista
    }

    private void saveChoices(){
        SharedPreferences.Editor editor = sharedPref.edit();
        json = gson.toJson(lista);  // jsoniin tallennetaan gsonin avulla listan sisältö talteen
        editor.putString("choices", json);
        editor.commit();
    }

    private void showNewList(){
        ArrayAdapter adapter = new ArrayAdapter(this, android.R.layout.simple_list_item_1, lista);
        lv.setAdapter(adapter);
    }
}
